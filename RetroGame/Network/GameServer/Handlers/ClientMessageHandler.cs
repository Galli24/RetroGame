using GameServer.Configuration;
using GameServer.Lobby;
using GameServer.Utils;
using LibNetworking;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Sockets;

namespace GameServer.Server
{
    class ClientMessageHandler
    {
        private readonly Config _config;
        private readonly LobbyManager _lobbyManager;

        private static readonly HttpClient _client = new HttpClient();

        public ClientMessageHandler(Config config, LobbyManager lobbyManager)
        {
            _config = config;
            _lobbyManager = lobbyManager;
            NetworkCallbacks.OnClientMessage += OnClientMessage;
        }

        // TODO
        private void OnClientMessage(Socket client, ClientMessage message)
        {
            switch (message.MessageTarget)
            {
                case MessageTarget.CONNECT:
                    OnConnectMessage(client, (ClientConnectMessage)message);
                    break;
                case MessageTarget.LOBBBY:
                    // TODO: Lobby message messages & handler
                    break;
                case MessageTarget.GAME:
                    // TODO: Game messages & handler
                    break;
                default:
                    break;
            }
        }

        private async void OnConnectMessage(Socket client, ClientConnectMessage message)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", message.Token);
            var body = "{\"id\":\"" + message.Id + "\",\"username\":\"" + message.Username + "\"}";

            int statusCode = -1;
            string responseData = string.Empty;

            try
            {
                using var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                using var response = await _client.PostAsync(_config.AuthServerURI + "/api/v1/users/verify", content).ConfigureAwait(false);
                statusCode = (int)response.StatusCode;
                responseData = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                new ServerConnectMessage(client, false, "An internal exception occured, please try again later").Send();
                TCPServer.CloseSocket(client);
                return;
            }

            if (statusCode == 200)
                new ServerConnectMessage(client, true, "").Send();
            else if (statusCode == 401)
            {
                var jsonData = JObject.Parse(responseData);
                var error = jsonData.ContainsKey("error") ? jsonData["error"].ToString() : string.Empty;

                ServerConnectMessage response = error switch
                {
                    "User does not exist" or "User does not match" => new ServerConnectMessage(client, false, "Wrong username or password"),
                    _ => new ServerConnectMessage(client, false, "Something unexpected happened, please try again"),
                };
                response.Send();
                TCPServer.CloseSocket(client);
            } else
            {
                new ServerConnectMessage(client, false, "An internal exception occured, please try again later").Send();
                TCPServer.CloseSocket(client);
            }
        }
    }
}
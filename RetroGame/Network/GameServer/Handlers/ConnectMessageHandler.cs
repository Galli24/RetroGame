using GameServer.Server;
using GameServer.Utils;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace GameServer.Handlers
{
    class ConnectMessageHandler
    {
        private static readonly HttpClient _client = new HttpClient();

        public static void OnConnectMessage(SocketState client, ClientMessage message)
        {
            switch (message.ClientMessageType)
            {
                case ClientMessageType.CONNECT:
                    OnConnect(client, (ClientConnectMessage)message);
                    break;
                case ClientMessageType.RECONNECT:
                    OnReconnect(client, (ClientReconnectMessage)message);
                    break;
                default:
                    return;
            }
        }

        private static async void OnConnect(SocketState client, ClientConnectMessage message)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", message.Token);
            var body = "{\"id\":\"" + message.Id + "\",\"username\":\"" + message.Username + "\"}";

            int statusCode = -1;
            string responseData = string.Empty;

            try
            {
                using var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                using var response = await _client.PostAsync(GlobalManager.Instance.Config.AuthServerURI + "/api/v1/users/verify", content).ConfigureAwait(false);
                statusCode = (int)response.StatusCode;
                responseData = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                new ServerConnectedMessage(client.Socket, false, "An internal exception occured, please try again later").Send();
                TCPServer.CloseSocketState(client);
                return;
            }

            if (statusCode == 200)
            {
                client.IsAuthenticated = true;
                client.UID = message.Id;
                client.Username = message.Username;
                new ServerConnectedMessage(client.Socket, true, "").Send();
            }
            else if (statusCode == 401)
            {
                var jsonData = JObject.Parse(responseData);
                var error = jsonData.ContainsKey("error") ? jsonData["error"].ToString() : string.Empty;

                ServerConnectedMessage response = error switch
                {
                    "User does not exist" or "User does not match" => new ServerConnectedMessage(client.Socket, false, "Wrong username or password"),
                    _ => new ServerConnectedMessage(client.Socket, false, "Something unexpected happened, please try again"),
                };
                response.Send();
                TCPServer.CloseSocketState(client);
            }
            else
            {
                new ServerConnectedMessage(client.Socket, false, "An internal exception occured, please try again later").Send();
                TCPServer.CloseSocketState(client);
            }
        }

        private static void OnReconnect(SocketState client, ClientReconnectMessage message)
        {
            OnConnect(client, new ClientConnectMessage(message.Token, message.Id, message.Username));

            // TODO: Implement reonnection
            // Requires Lobbies
        }
    }
}

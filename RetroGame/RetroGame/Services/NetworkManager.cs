using LibNetworking.Messages.Client;
using Newtonsoft.Json.Linq;
using RetroGame.Networking;
using RetroGame.Networking.Handlers;
using RetroGame.Utils;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetroGame.Services
{
    class NetworkManager
    {
        #region Singleton

        private static NetworkManager _instance;
        public static NetworkManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NetworkManager();
                return _instance;
            }
        }

        #endregion

        #region Members

        private readonly TCPClient _tcpClient;
        public TCPClient TCPClient
        {
            get
            {
                return _tcpClient;
            }
        }

        private ServerMessageHandler _serverMessageHandler;

        private static readonly HttpClient _client = new HttpClient();

        private readonly string _authServerURI;
        private readonly string _authEndpoint = "users/authenticate";
        private readonly string _registerEndpoint = "users/register";

        #endregion

        private NetworkManager()
        {
            _tcpClient = new TCPClient("176.164.108.185", 27015);
            _serverMessageHandler = new ServerMessageHandler(_tcpClient);

            _authServerURI = "http://176.164.108.185:3000/api/v1/";
        }

        #region Connect logic

        public async Task<string> Register(string email, string username, string password)
        {
            return await Task.Run(() => RegisterAsync(email, username, password));
        }

        private async Task<string> RegisterAsync(string email, string username, string password)
        {
            try
            {
                Trace.WriteLine("Register");

                var uri = _authServerURI + _registerEndpoint;
                var body = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\",\"email\":\"" + email + "\"}";

                int statusCode = -1;
                string responseData = string.Empty;

                using var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                using var response = await _client.PostAsync(uri, content).ConfigureAwait(false);
                statusCode = (int)response.StatusCode;
                responseData = await response.Content.ReadAsStringAsync();

                if (statusCode == 200)
                {
                    Trace.WriteLine("Registered");
                    return "Registered";
                }
                else
                {
                    var jsonData = JObject.Parse(responseData);
                    var error = jsonData["error"].ToString();

                    Trace.WriteLine(error);
                    return error;
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                return "Unable to reach the server";
            }
        }

        public async Task<string> Connect(string username, string password)
        {
            return await Task.Run(() => ConnectAsync(username, password));
        }

        private async Task<string> ConnectAsync(string username, string password)
        {
            try
            {
                Trace.WriteLine("Connect");

                var uri = _authServerURI + _authEndpoint;
                var body = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";

                int statusCode = -1;
                string responseData = string.Empty;

                using var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                using var response = await _client.PostAsync(uri, content).ConfigureAwait(false);
                statusCode = (int)response.StatusCode;
                responseData = await response.Content.ReadAsStringAsync();

                if (statusCode == 200)
                {
                    var jsonData = JObject.Parse(responseData);
                    var id = jsonData["id"].ToString();
                    var token = jsonData["token"].ToString();

                    Trace.WriteLine("Connected");

                    UserManager.Instance.Id = id;
                    UserManager.Instance.Username = username;

                    _tcpClient.Connect();
                    new ClientConnectMessage(token, id, username).Send();

                    return "Connected";
                }
                else
                {
                    var jsonData = JObject.Parse(responseData);
                    var error = jsonData["error"].ToString();

                    Trace.WriteLine(error);
                    return error;
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                return "Unable to reach the server";
            }
        }

        #endregion

        #region Lobby logic

        public void CreateLobby(string name, string password)
        {
            Task.Run(() => new ClientLobbyCreateMessage(name, password).Send());
        }

        public void JoinLobby(string name, string password)
        {
            Task.Run(() => new ClientLobbyJoinMessage(name, password).Send());
        }

        public void LeaveLobby()
        {
            Task.Run(() => new ClientLobbyLeaveMessage().Send());
        }

        #endregion
    }
}

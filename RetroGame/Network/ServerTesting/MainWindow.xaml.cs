using LibNetworking;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;


namespace ServerTesting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TCPClient _tcpClient;
        private static readonly HttpClient _client = new HttpClient();

        public MainWindow()
        {
            _tcpClient = new TCPClient("127.0.0.1", 27015);
            _tcpClient.OnServerMessage += OnServerMessage;

            InitializeComponent();
        }

        private void OnServerMessage(ServerMessage message)
        {
            switch (message.ServerMessageType)
            {
                case ServerMessageType.CONNECTED:
                    var connectMessage = (ServerConnectedMessage)message;
                    if (connectMessage.Authorized)
                    {
                        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, (System.Action)delegate ()
                        {
                            Error.Text = "Connected";
                            button_connect.Visibility = Visibility.Visible;
                        });
                    } else
                    {
                        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, (System.Action)delegate ()
                        {
                            Error.Text = connectMessage.Reason;
                            button_connect.Visibility = Visibility.Visible;
                        });
                    }
                    break;
                default:
                    break;
            }
        }

        private async void Connect(object sender, RoutedEventArgs e)
        {
            try
            {
                Error.Text = "Connecting...";
                button_connect.Visibility = Visibility.Hidden;
                var username = UsernameBox.Text;
                var password = PasswordBox.Text;

                // Auth Server
                var uri = "https://localhost:5001/api/v1/users/authenticate";
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

                    _tcpClient.Connect();
                    _tcpClient.SendClientMessage(new ClientConnectMessage(token, id, username));
                } else
                {
                    var jsonData = JObject.Parse(responseData);
                    var error = jsonData["error"].ToString();

                    await Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, (System.Action)delegate ()
                    {
                        Error.Text = error;
                        button_connect.Visibility = Visibility.Visible;
                    });
                }

            } catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.InnerException);
                Trace.WriteLine(ex.StackTrace);
                await Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, (System.Action)delegate ()
                 {
                     Error.Text = "An internal error occured";
                     button_connect.Visibility = Visibility.Visible;
                 });
            }
        }
    }
}

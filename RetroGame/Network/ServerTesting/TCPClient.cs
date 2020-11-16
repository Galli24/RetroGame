using LibNetworking;
using LibNetworking.Messages;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ServerTesting
{
    public sealed class TCPClient
    {

        #region Members

        private readonly IPAddress _serverIP;
        private readonly ushort _serverPort;
        private Socket _socket;

        private class SocketState
        {
            public Socket Socket;
            public byte[] SizeBuffer;
            public byte[] Buffer;
            public MemoryStream Data;
        }

        #endregion

        public delegate void OnServerMessageDelegate(ServerMessage message);
        public OnServerMessageDelegate OnServerMessage { get; set; }

        #region Logic

        public TCPClient(string ip, ushort serverPort)
        {
            _serverIP = IPAddress.Parse(ip);
            _serverPort = serverPort;
        }

        private void Close()
        {
            if (_socket != null)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _socket = null;
            }
        }

        public void Connect()
        {
            try
            {
                if (_socket != null)
                {
                    Close();
                }

                string localHostName = Dns.GetHostName();
                IPHostEntry localMachineInfo = Dns.GetHostEntry(localHostName);
                IPEndPoint serverEndpoint = new IPEndPoint(_serverIP, _serverPort);
                IPEndPoint myEndpoint = new IPEndPoint(IPAddress.Any, 0);

                _socket = new Socket(myEndpoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(serverEndpoint);
                Trace.WriteLine("Server connection established");

                Receive(_socket);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Trace.WriteLine(e.StackTrace);
            }
        }

        public void SendClientMessage(ClientMessage message)
        {
            if (_socket != null)
            {
                Trace.WriteLine($"Sending a packet with a size of {Message.SerializeToBytes(message).Length} bytes to {_socket.RemoteEndPoint}");

                var serializedMessage = Message.SerializeToBytes(message);
                var sizeBytes = BitConverter.GetBytes(serializedMessage.Length);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(sizeBytes);

                _socket.Send(sizeBytes);
                _socket.Send(serializedMessage);
            }
        }

        private void Receive(Socket socket)
        {
            if (_socket == null)
                return;

            var state = new SocketState();

            try
            {
                state.Socket = socket;
                state.SizeBuffer = new byte[4];

                state.Socket.BeginReceive(state.SizeBuffer, 0, 4, SocketFlags.None, new AsyncCallback(OnReceivePacketSize), state);
            }
            catch
            {
                Close();
            }
        }

        private void OnReceivePacketSize(IAsyncResult result)
        {
            var state = (SocketState)result.AsyncState;
            var socket = state.Socket;

            try
            {
                int readBytes = socket.EndReceive(result);
                if (readBytes == 4)
                {
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(state.SizeBuffer);

                    var packetSize = BitConverter.ToInt32(state.SizeBuffer);
                    state.Buffer = new byte[packetSize + 1];
                    socket.BeginReceive(state.Buffer, 0, packetSize, SocketFlags.None, new AsyncCallback(OnReceivePacket), state);
                }
                else
                {
                    Close();
                }
            }
            catch
            {
                Close();
            }
        }

        private void OnReceivePacket(IAsyncResult result)
        {
            var state = (SocketState)result.AsyncState;
            var socket = state.Socket;

            try
            {
                int readBytes = socket.EndReceive(result);
                if (readBytes > 0)
                {
                    state.Data = new MemoryStream(state.Buffer, 0, readBytes, false, true);
                    var message = Message.DeserializeFromStream(state.Data);
                    if (message.MessageType == MessageType.SERVER)
                        OnServerMessage((ServerMessage)message);
                    state.SizeBuffer = new byte[4];
                    state.Socket.BeginReceive(state.SizeBuffer, 0, 4, SocketFlags.None, new AsyncCallback(OnReceivePacketSize), state);
                }
                else
                {
                    Close();
                }
            }
            catch
            {
                Close();
            }
        }

        #endregion
    }
}

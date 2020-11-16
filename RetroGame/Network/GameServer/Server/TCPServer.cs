using LibNetworking;
using LibNetworking.Messages;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace GameServer.Server
{
    public sealed class TCPServer
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

        #region Logic

        public TCPServer(string ip, ushort port)
        {
            _serverIP = IPAddress.Parse(ip);
            _serverPort = port;
        }

        public void Start()
        {
            var endpoint = new IPEndPoint(_serverIP, _serverPort);
            _socket = new Socket(endpoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(endpoint);
            _socket.Listen((int)SocketOptionName.MaxConnections);
            _socket.BeginAccept(new AsyncCallback(OnClientConnect), _socket);

            Console.WriteLine($"Server is listening on {_serverIP}:{_serverPort}");
        }

        private void OnClientConnect(IAsyncResult result)
        {
            var state = new SocketState();

            try
            {
                var socket = (Socket)result.AsyncState;
                state.Socket = socket.EndAccept(result);
                state.SizeBuffer = new byte[4];
                state.Socket.BeginReceive(state.SizeBuffer, 0, 4, SocketFlags.None, new AsyncCallback(OnReceivePacketSize), state);
                _socket.BeginAccept(new AsyncCallback(OnClientConnect), result.AsyncState);
            }
            catch
            {
                CloseSocketState(state);
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
                    CloseSocketState(state);
                }
            }
            catch
            {
                CloseSocketState(state);
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
                    if (message.MessageType == MessageType.CLIENT)
                        NetworkCallbacks.OnClientMessage(socket, (ClientMessage)message);
                    if (IsSocketDisposed(state.Socket))
                        return;
                    state.SizeBuffer = new byte[4];
                    state.Socket.BeginReceive(state.SizeBuffer, 0, 4, SocketFlags.None, new AsyncCallback(OnReceivePacketSize), state);
                }
                else
                {
                    CloseSocketState(state);
                }
            }
            catch
            { 
                CloseSocketState(state);
            }
        }

        private static bool IsSocketDisposed(Socket socket)
        {
            var bfIsDisposed = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty;
            var field = socket.GetType().GetProperty("Disposed", bfIsDisposed);
            return (bool)field.GetValue(socket, null);
        }

        public static void SendServerMessage(Socket client, ServerMessage message)
        {
            Console.WriteLine($"Sending a packet with a size of {Message.SerializeToBytes(message).Length} bytes to {client.RemoteEndPoint}");

            var serializedResponse = Message.SerializeToBytes(message);
            var sizeBytes = BitConverter.GetBytes(serializedResponse.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(sizeBytes);

            client.Send(sizeBytes);
            client.Send(serializedResponse);
        }

        public static void CloseSocket(Socket socket) => socket.Close();

        private static void CloseSocketState(SocketState ci) => ci.Socket.Close();


        #endregion
    }
}
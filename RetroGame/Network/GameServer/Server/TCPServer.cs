﻿using LibNetworking.Messages;
using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using LibNetworking.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace GameServer.Server
{
    public sealed class TCPServer
    {
        #region Members

        private readonly IPAddress _serverIP;
        private readonly ushort _serverPort;
        private Socket _socket;

        public delegate void OnClientMessageDelegate(SocketState client, ClientMessage message);
        public OnClientMessageDelegate OnClientMessage;

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

            try
            {
                int readBytes = state.Socket.EndReceive(result);
                if (readBytes == 4)
                {
                    state.PacketSize = BitConverter.ToInt32(state.SizeBuffer);
                    state.Offset = 0;
                    state.Buffer = new byte[state.PacketSize];
                    state.Socket.BeginReceive(state.Buffer, 0, state.PacketSize, SocketFlags.None, new AsyncCallback(OnReceivePacket), state);
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

            try
            {
                int readBytes = state.Socket.EndReceive(result);
                if (readBytes > 0)
                {
                    state.Offset += readBytes;
                    if (state.Offset != state.PacketSize)
                        state.Socket.BeginReceive(state.Buffer, state.Offset, state.PacketSize - state.Offset, SocketFlags.None, new AsyncCallback(OnReceivePacket), state);
                    else
                    {
                        state.Data = new MemoryStream(state.Buffer, 0, state.PacketSize, false, true);
                        var message = Message.DeserializeFromStream(state.Data);
                        if (message.MessageType == MessageType.CLIENT)
                            OnClientMessage(state, (ClientMessage)message);
                        if (state.IsSocketDisposed)
                            return;
                        state.SizeBuffer = new byte[4];
                        state.Socket.BeginReceive(state.SizeBuffer, 0, 4, SocketFlags.None, new AsyncCallback(OnReceivePacketSize), state);
                    }
                }
                else
                {
                    CloseSocketState(state);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                CloseSocketState(state);
            }
        }

        public static void SendServerMessage(Socket client, ServerMessage message)
        {
            // Console.WriteLine($"Sending a packet with a size of {Message.SerializeToBytes(message).Length} bytes to {client.RemoteEndPoint}");
            if (message.MessageTarget != MessageTarget.GAME)
                Console.WriteLine($"Response {message.ServerMessageType}");

            var serializedResponse = Message.SerializeToBytes(message);
            var sizeBytes = BitConverter.GetBytes(serializedResponse.Length);
            var bytesToSend = sizeBytes.Concat(serializedResponse).ToArray();

            client.Send(bytesToSend);
        }

        internal static void CloseSocketState(SocketState state)
        {
            var lobby = GlobalManager.Instance.LobbyManager.GetLobbyFromState(state);
            if (lobby != null)
                lobby.PlayerLeave(state);

            state.Socket.Close();
            state.IsSocketDisposed = true;
        }


        #endregion
    }
}

﻿using LibNetworking.Messages.Client;
using LibNetworking.Messages.Server;
using ProtoBuf;
using System;
using System.IO;

namespace LibNetworking.Messages
{
    public enum MessageType
    {
        UNDEFINED,
        SERVER,
        CLIENT
    }

    public enum MessageTarget
    {
        UNDEFINED,
        CONNECT,
        LOBBY,
        GAME,
    }

    [ProtoContract(SkipConstructor = true)]
    [ProtoInclude(1, typeof(ClientMessage))]
    [ProtoInclude(2, typeof(ServerMessage))]
    public abstract class Message
    {
        [ProtoMember(3)]
        public MessageType MessageType { get; private set; }
        [ProtoMember(4)]
        public DateTime Time { get; private set; }

        protected Message(MessageType messageType)
        {
            MessageType = messageType;
            Time = DateTime.UtcNow;
        }

        public override string ToString()
        {
            throw new NotImplementedException($"The class {GetType().Name} does not override the ToString() method");
        }

        public static Message DeserializeFromStream(MemoryStream zipStream)
        {
            return Serializer.Deserialize<Message>(zipStream);
        }

        public static byte[] SerializeToBytes(Message message)
        {
            using var rawStream = new MemoryStream();
            Serializer.Serialize(rawStream, message);
            return rawStream.ToArray();
        }
    }
}

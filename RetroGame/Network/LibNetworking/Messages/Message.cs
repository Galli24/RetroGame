using LibNetworking.Messages.Client;
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
            using var rawStream = new MemoryStream(zipStream.GetBuffer());
            return Serializer.DeserializeWithLengthPrefix<Message>(rawStream, PrefixStyle.Base128);
        }

        public static byte[] SerializeToBytes(Message message)
        {
            using var rawStream = new MemoryStream();
            Serializer.SerializeWithLengthPrefix(rawStream, message, PrefixStyle.Base128);
            return rawStream.ToArray();
        }
    }
}

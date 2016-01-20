using System;
using System.Runtime.Serialization;

namespace SocketClientServer_Server
{
    [Serializable]
    internal class SocketArgumentException : Exception
    {
        public SocketArgumentException()
        {
        }

        public SocketArgumentException(string message) : base(message)
        {
        }

        public SocketArgumentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SocketArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
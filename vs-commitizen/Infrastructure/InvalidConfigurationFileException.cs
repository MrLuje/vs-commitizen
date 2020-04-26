using System;
using System.Runtime.Serialization;

namespace vs_commitizen.Infrastructure
{
    [Serializable]
    internal class InvalidConfigurationFileException : Exception
    {
        public InvalidConfigurationFileException()
        {
        }

        public InvalidConfigurationFileException(string message) : base(message)
        {
        }

        public InvalidConfigurationFileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidConfigurationFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
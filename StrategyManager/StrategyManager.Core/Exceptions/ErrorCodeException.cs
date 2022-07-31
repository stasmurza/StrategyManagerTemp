using System.Runtime.Serialization;

namespace StrategyManager.Core.Exceptions
{
    public class ErrorCodeException : Exception
    {
        public int ErrorCode { get; set; }

        public ErrorCodeException(string message) : base(message)
        {
        }

        protected ErrorCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ErrorCodeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

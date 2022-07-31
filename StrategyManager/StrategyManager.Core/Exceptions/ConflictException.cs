using System.Runtime.Serialization;

namespace StrategyManager.Core.Exceptions
{
    public class ConflictException : ErrorCodeException
    {
        public ConflictException(string message)
            : base(message)
        {
        }

        public ConflictException() : base("Conflict")
        {
        }

        protected ConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

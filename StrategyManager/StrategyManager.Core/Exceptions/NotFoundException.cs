using System.Runtime.Serialization;

namespace StrategyManager.Core.Exceptions
{
    public class NotFoundException : ErrorCodeException
    {
        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException() : base("Not found")
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

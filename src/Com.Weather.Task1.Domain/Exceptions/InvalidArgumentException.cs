namespace Com.Weather.Task1.Domain.Exceptions
{
    public class InvalidArgumentException : DomainException
    {
        public InvalidArgumentException(string errorMessage) : base(errorMessage, null)
        {
        }

        public override int ErrorCode => 400;
    }
}

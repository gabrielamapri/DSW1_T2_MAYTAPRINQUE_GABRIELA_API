using System;

namespace Library.Domain.Exceptions
{
    public class DuplicateEntityException : DomainException
    {
        public DuplicateEntityException()
        {
        }

        public DuplicateEntityException(string message) : base(message)
        {
        }

        public DuplicateEntityException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

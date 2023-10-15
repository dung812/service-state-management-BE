using Microsoft.AspNetCore.Http;

namespace ProductionManagement.Exceptions
{
    public class PrimaryKeyConstraintException : CustomException
    {
        public PrimaryKeyConstraintException(string message) : base(StatusCodes.Status409Conflict, message) { }
    }
}

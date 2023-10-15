using Microsoft.AspNetCore.Http;

namespace ProductionManagement.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message) : base(StatusCodes.Status404NotFound, message) { }
    }
}

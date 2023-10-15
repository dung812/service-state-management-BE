using System;

namespace ProductionManagement.Exceptions
{
    public abstract class CustomException : Exception
    {
        public int StatusCode { get; set; } = 500;

        protected CustomException(int statusCode ,string message) : base(message) {
            StatusCode = statusCode;
        }
    }
}

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductionManagement.DataContract.Common;
using ProductionManagement.Exceptions;

namespace ProductionManagement.API.Configurations.Middleware
{
	public class GlobalExceptionHandler
	{
		private readonly RequestDelegate _next;

		public GlobalExceptionHandler(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, ILogger<GlobalExceptionHandler> logger)
		{
			try
			{
				//Next to the remaining middleware
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex, logger);
			}
		}
		private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger _logger)
		{
			var messageToResponse = exception switch
			{
				CustomException ex => new ErrorResponse { StatusCode = ex.StatusCode, Message = ex.Message },
				ArgumentNullException ex => new ErrorResponse { StatusCode = StatusCodes.Status400BadRequest, Message = ex.Message },
				ArgumentException ex => new ErrorResponse { StatusCode = StatusCodes.Status400BadRequest, Message = ex.Message },
				SecurityTokenException ex => new ErrorResponse { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message },
				InvalidOperationException ex => new ErrorResponse { StatusCode = StatusCodes.Status400BadRequest, Message = ex.Message },
				SqlException => new ErrorResponse { StatusCode = StatusCodes.Status500InternalServerError, Message = "Some unexpected SQL errors has occured" },
				DbUpdateException => new ErrorResponse { StatusCode = StatusCodes.Status500InternalServerError, Message = "There are some errors when update the database" },
				_ => new ErrorResponse { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal server error" },
			};

			_logger.LogError(exception.InnerException?.Message ?? exception.Message);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = messageToResponse.StatusCode;
			await context.Response.WriteAsync(messageToResponse.ToString());
		}
	}
}

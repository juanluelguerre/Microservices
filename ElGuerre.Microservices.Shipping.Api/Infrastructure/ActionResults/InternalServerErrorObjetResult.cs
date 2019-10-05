using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElGuerre.Microservices.Shipping.Api.Infrastructure.ActionResults
{
	public class InternalServerErrorObjectResult : ObjectResult
	{
		public InternalServerErrorObjectResult(object error)
			: base(error)
		{
			StatusCode = StatusCodes.Status500InternalServerError;
		}
	}
}

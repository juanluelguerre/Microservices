using ElGuerre.Microservices.Sales.Api.Application.Infrastructure;
using ElGuerre.Microservices.Sales.Api.Application.Models;
using ElGuerre.Microservices.Sales.Api.Application.ViewModels;
using ElGuerre.Microservices.Sales.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Controllers
{
	[SwaggerTag("Sales")]
	[Route("/api/v1/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		private readonly OrdersContext _modulo1Context;

		public ValuesController(OrdersContext context)
		{
			_modulo1Context = context ?? throw new ArgumentNullException(nameof(context));
		}

        /// <summary>
        /// probamos
        /// </summary>
        /// <remarks>
        /// remarks
        /// </remarks>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(PaginatedItemsViewModel<Order>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(int pageSize = 10, int pageIndex = 0, string ids = null)
		{
            if (!string.IsNullOrEmpty(ids))
			{
				var items = new string[] { "value1", "value2" };

				if (!items.Any())
				{
					return BadRequest("ids value invalid. Must be comma-separated list of numbers");
				}

				return Ok(items);
			}

			var totalItems = await _modulo1Context.Orders
				.LongCountAsync();

			var orders = await _modulo1Context.Orders
				.OrderBy(c => c.Name)
				.Skip(pageSize * pageIndex)
				.Take(pageSize)
				.ToListAsync();

			var model = new PaginatedItemsViewModel<Order>(pageIndex, pageSize, totalItems, orders.Select( o => new Order() { OrderId = o.Id, Name = o.Name }));

			return Ok(model);
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public ActionResult<string> GetById(int id)
		{
			return "value";
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}

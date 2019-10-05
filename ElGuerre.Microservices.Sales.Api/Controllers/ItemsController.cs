using ElGuerre.Microservices.Sales.Api.Application.Models;
using ElGuerre.Microservices.Sales.Api.Application.Services;
using ElGuerre.Microservices.Sales.Api.Application.ViewModels;
using ElGuerre.Microservices.Sales.Api.Infrastructure;
using ElGuerre.Microservices.Sales.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Controllers
{
	[SwaggerTag("Sales")]
	[Route("/api/v1/[controller]")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private IOrdersService _ordersService;

		public OrdersController(IOrdersService ordersService)
		{
			_ordersService = ordersService;
		}

		/// <summary>
		/// Obtiene los <paramref name="pageSize"/> elementos de la página <paramref name="pageIndex"/>
		/// </summary>
		/// <remarks>
		/// Si el tamaño de <paramref name="pageIndex"/> es inferior a 0, se presupone un valor 1, como valor predeterminado.
		/// De igual forma, para <paramref name="pageIndex"/> se presupone un valor predterminado de 10.
		/// </remarks>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <param name="ids"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(PaginatedItemsViewModel<Order>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetOrders(int pageSize = 10, int pageIndex = 0, string ids = null)
		{
			var pagedItems = await _ordersService.GetItems(10, 1);
			var model = new PaginatedItemsViewModel<Order>(pagedItems.PageIndex, pagedItems.PageSize, pagedItems.Total, pagedItems.Items);
			return Ok(model);
		}

		/// <summary>
		/// Obtiene un elemento
		/// </summary>
		/// <param name="id">Identificador del elemento a obtener</param>
		/// <returns>Elemento con el id que se recibe como parámetro</returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> GetOrder(int id)
		{
			return Ok(_ordersService.GetItem(id));
		}

		[HttpPost()]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> UpdateOrder([FromBody]Order order)
		{
			bool result = await _ordersService.UpdateItem(order, false);
			return Ok(result);
		}
	}
}

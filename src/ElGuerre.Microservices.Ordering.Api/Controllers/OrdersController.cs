using ElGuerre.Microservices.Ordering.Api.Application.Commands;
using ElGuerre.Microservices.Ordering.Api.Application.Models;
using ElGuerre.Microservices.Ordering.Api.Application.Queries;
using ElGuerre.Microservices.Ordering.Api.Application.ViewModels;
using ElGuerre.Microservices.Ordering.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Controllers
{
	[SwaggerTag("Orders")]
	[Route("/api/v1/[controller]")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		// private IOrdersService _ordersService;
		private readonly IMediator _mediator;

		public OrdersController(IMediator mediator /*IOrdersService ordersService*/)
		{
			// _ordersService = ordersService;
			_mediator = mediator;
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
		[ProducesResponseType(typeof(PagedItemsViewModel<Order>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetOrders(int pageIndex = 0, int pageSize = 10, string ids = null)
		{
			//var pagedItems = await _ordersService.GetOrders(pageIndex <= 0 ? 1 : pageIndex, pageSize <= 0 ? 10 : pageSize);
			//var model = new PagedItemsViewModel<Order>(pagedItems.PageIndex, pagedItems.PageSize, pagedItems.TotalItems, pagedItems.Items);
			//return Ok(model);

			var query = new OrdersPagedQuery(pageIndex <= 0 ? 1 : pageIndex, pageSize <= 0 ? 10 : pageSize);
			var result = await _mediator.Send(query);
			return Ok(result);
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
			// return Ok(await _ordersService.GetOrder(id));
			var query = new OrdersByIdQuery(id);
			var result = await _mediator.Send(query); ;
			return Ok(result);
		}

		[HttpPost("{orderId}")]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> ReadyToPay(int orderId)
		{
			var command = new OrderToBillCommand(orderId);
			var result = await _mediator.Send(command);
			return Ok(result);
		}

		[HttpPost]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> UpdateOrder([FromBody]Order order)
		{
			//bool result = await _ordersService.PublishToBill(orderId);
			//return Ok(result);

			return Ok(await Task.FromResult(true));
		}
	}
}

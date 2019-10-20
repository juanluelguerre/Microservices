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
		private readonly IMediator _mediator;

		public OrdersController(IMediator mediator)
		{
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
		[ProducesResponseType(typeof(PagedItemsViewModel<OrderModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> GetOrders(int pageIndex = 0, int pageSize = 10, string ids = null)
		{
			var command = new OrderPagedQuery(pageIndex <= 0 ? 1 : pageIndex, pageSize <= 0 ? 10 : pageSize);
			var result = await _mediator.Send(command);
			return Ok(result);
		}

		/// <summary>
		/// Obtiene un elemento
		/// </summary>
		/// <param name="id">Identificador del elemento a obtener</param>
		/// <returns>Elemento con el id que se recibe como parámetro</returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(OrderModel), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> GetOrder(int orderId)
		{
			var command = new OrderByIdQuery(orderId);
			var result = await _mediator.Send(command);
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
		public async Task<IActionResult> AddOrder([FromBody]OrderModel order)
		{
			var command = new OrderCreateCommand(order.OrderItems,
				order.UserId, order.UserName,
				order.City, order.Street, order.Country, order.ZipCode, order.CardNumber,
				order.CardHolderName, order.CardExpiration, order.CardTypeId);

			var result = await _mediator.Send(command);

			return Ok(result);
		}
	}
}

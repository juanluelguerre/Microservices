using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ElGuerre.Microservices.Billing.Api.Infrastructure;
using ElGuerre.Microservices.Billing.Api.Application.Services;
using ElGuerre.Microservices.Billing.Api.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using ElGuerre.Microservices.Billing.Api.Services;

namespace ElGuerre.Microservices.Billing.Api.Controllers
{
    [SwaggerTag("Billing")]
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private IItemsService _itemsService;

        public ItemsController(IItemsService elementosService)
        {
            _itemsService = elementosService;
        }
      
    }
}

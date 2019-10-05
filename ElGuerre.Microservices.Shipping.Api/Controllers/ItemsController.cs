using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ElGuerre.Microservices.Shipping.Api.Infrastructure;
using ElGuerre.Microservices.Shipping.Api.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ElGuerre.Microservices.Shipping.Api.Controllers
{
    [SwaggerTag("Shipping")]
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
       
    }
}

using ElGuerre.Microservices.Billing.Api.Infrastructure;
using ElGuerre.Microservices.Billing.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Billing.Api.Application.Services
{
    public class ItemsService : IItemsService
    {
        private readonly BillingContext _modulo1Context;

        public ItemsService (BillingContext context)
        {
            _modulo1Context = context;
        }
      


		// pagar
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Models
{
	[JsonObject("CardType")]
	public class CardTypeModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

}

using ElGuerre.Microservices.Ordering.Api.Application.Models;
using ElGuerre.Microservices.Sales.Api.IntegrationTests.Infrastructure;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ElGuerre.Microservices.Sales.Api.IntegrationTests
{
	public class OrdersIntegrationTests : BaseTest
	{
		private readonly string _baseUrl;

		public OrdersIntegrationTests(CompositionRootFixture fixture) : base(fixture)
		{
			_baseUrl = "api/orders";
			// AutoMapper.Mapper.AssertConfigurationIsValid();
			// AutoMapper.Mapper.Reset();
			// MappingConfig.RegisterMaps();

			// https://stackoverflow.com/questions/40275195/how-to-setup-automapper-in-asp-net-core
			//var profile = new ProjectProfile();

			//var config = new MapperConfiguration(ex => ex.AddProfile(profile));
			//var mapper = new Mapper(config);

			//(mapper as IMapper).ConfigurationProvider.AssertConfigurationIsValid();
		}

		[Fact]
		public async Task GetAllTest()
		{
			var response = await Fixture.Client.GetAsync(_baseUrl);
			response.EnsureSuccessStatusCode();

			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			var content = await response.Content.ReadAsStringAsync();

			var orders = JsonConvert.DeserializeObject<IEnumerable<OrderModel>>(content);
			Assert.NotNull(orders);
			Assert.True(orders.Any());
		}

		[Theory]
		[InlineData(1)]
		//[InlineData(2)]       
		public async Task GetTest(int orderId)
		{
			var response = await Fixture.Client.GetAsync($"{_baseUrl}/{orderId}");
			response.EnsureSuccessStatusCode();

			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			var content = await response.Content.ReadAsStringAsync();

			var order = JsonConvert.DeserializeObject<OrderModel>(content);
			Assert.NotNull(order);
		}

		[Theory]
		[InlineData(33)]
		public async Task GetNotFoundTest(int orderId)
		{
			var response = await Fixture.Client.GetAsync($"{_baseUrl}/{orderId}");
			Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());

			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public async Task PostTest()
		{
			var content = GetOrderSample(15);

			var response = await Fixture.Client.PostAsync(_baseUrl, content);
			response.EnsureSuccessStatusCode();
		}

		[Theory]
		[InlineData(2)]
		public async Task PutTest(int orderId)
		{
			var content = GetOrderSample(orderId);

			var response = await Fixture.Client.PutAsync($"{_baseUrl}/{orderId}", content);
			response.EnsureSuccessStatusCode();
		}

		[Theory]
		[InlineData(3)]
		public async Task DeleteTest(int orderId)
		{
			var response = await Fixture.Client.DeleteAsync($"{_baseUrl}/{orderId}");
			response.EnsureSuccessStatusCode();
		}

		[Theory]
		[InlineData(33)]
		public async Task DeleteNotFoundTest(int orderId)
		{
			var response = await Fixture.Client.DeleteAsync($"{_baseUrl}/{orderId}");
			Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
		}

		private static StringContent GetOrderSample(int orderId)
		{
			var proj = new OrderModel()
			{
				OrderNumber = orderId,
				Name = "Sample Order 1"
			};

			var content = new StringContent(
				JsonConvert.SerializeObject(proj),
				Encoding.UTF8,
				"application/json");
			return content;
		}
	}
}

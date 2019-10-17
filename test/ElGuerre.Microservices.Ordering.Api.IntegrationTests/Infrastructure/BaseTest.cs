using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ElGuerre.Microservices.Sales.Api.IntegrationTests.Infrastructure
{
		public class BaseTest : IClassFixture<CompositionRootFixture>
		//public class BaseTest : IClassFixture<WebAppFactory<Startup>>
		{
			protected readonly CompositionRootFixture Fixture;

			public BaseTest(CompositionRootFixture fixture)
			{
				Fixture = fixture;
			}
		}
	}


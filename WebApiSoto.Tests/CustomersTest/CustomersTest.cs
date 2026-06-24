using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiSoto.API.Middleware.Exceptions;
using WebApiSoto.Application.Common.DTOs.Customers;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Infrastructure.DbTrigger.Exceptions;
using WebApiSoto.Tests.Contextos;

namespace WebApiSoto.Tests.CustomersTest
{
    [TestFixture]
    public class CustomersTest
    {
        private CancellationToken ct;
        private HttpClient _client;

        [SetUp]
        public async Task SetUp()
        {
            var Factory = new ApiFactory();
            _client = Factory.CreateClient();

            // ── Autenticación: obtenemos un token real antes de cada prueba ──
            var loginPayload = new
            {
                userName = "AdminSoto",  
                password = "AdminSoto2026"  
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/Auth", loginPayload);
            loginResponse.EnsureSuccessStatusCode();

            using var stream = await loginResponse.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            var token = doc.RootElement
                .GetProperty("value")
                .GetProperty("token")
                .GetString();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        [Test]
        public async Task DebeRetornarListaYTotalDeRegistros()
        {
            // Arrange
            var Filters = new FiltersDto()
            {
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var respuesta = await _client.GetFromJsonAsync<Result<PaginationList<CustomersDto>>>("/api/customers");

            // Assert
            Assert.That(respuesta.StatusCode, Is.EqualTo(200));
            Assert.That(respuesta.Value, Is.Not.Null);
            Assert.That(respuesta.Value.Items.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task AddAsync()
        {
            try
            {
                // Arrange
                // Generamos una cédula con formato nicaragüense válido (001-DDMMYY-NNNNL)
                // pero con un correlativo aleatorio, para que sea única en cada ejecución
                // y la prueba no choque con un registro ya existente (409 Conflict).
                var random = new Random();
                var correlativo = random.Next(1000, 9999);
                var dniUnico = $"041-291108-{correlativo}U";

                var mock = new CreateCustomerDto()
                {
                    Name = "Fermin Perez",
                    City = "Jinotepe",
                    DNI = dniUnico,
                    Address = "En frente el hospital",
                    Phone = "58088332"
                };

                if (string.IsNullOrEmpty(mock.Name))
                    throw new InputInvalidoException("El nombre del Cliente no puede estar vacio", 400);

                // Act
                var response = await _client.PostAsJsonAsync("/api/customers", mock);
                var result = await response.Content.ReadFromJsonAsync<Result<CustomersDto>>();

                // Assert HTTP
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                // Assert Result
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.Not.Null);
                // Assert data
                Assert.That(result.Value.CustomerName, Is.EqualTo(mock.Name));
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
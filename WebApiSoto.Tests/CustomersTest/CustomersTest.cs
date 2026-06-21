using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
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
        public void SetUp()
        {
            var Factory = new ApiFactory();
            _client = Factory.CreateClient();
           
        }

        [Test]
        public async Task DebeRetornarListaYTotalDeRegistros()
        {

            //Arrange
            var Filters = new FiltersDto()
            {
                PageNumber = 1,
                PageSize = 10
            };
            

            //Act
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
                //Arrange, Trabajaremos dentro de una transaccion para no manchar la bd real
               
                //Creamos El Dto de entrada

                var mock = new CreateCustomerDto()
                {
                    Name = "Jose Suarez",
                    City = "Jinotepe",
                    DNI = "041-291108-9003U",
                    Address = "En frente el hospital",
                    Phone = "58088332"

                };

                if (string.IsNullOrEmpty(mock.Name))
                    throw new InputInvalidoException("El nombre del Cliente no puede estar vacio", 400);

                var response = await _client.PostAsJsonAsync("/api/customers", mock);

                var result = await response.Content
            .ReadFromJsonAsync<Result<CustomersDto>>();

                // Assert HTTP
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

                // Assert Result
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.Not.Null);

                // Assert data
                Assert.That(result.Value.CustomerName, Is.EqualTo(mock.Name));

                
            }
            catch(ApiException ex) 
            {
              Console.WriteLine(ex.Message);  
            }
        }
        
    }
}

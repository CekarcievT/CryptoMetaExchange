using BSDigitalPart2;
using BSDigitalPart2.DTOs;
using BSDigitalPart2.Infrastructure;
using IntegationTests.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace IntegationTests
{
    public class ExchangeControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ExchangeControllerTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(); // Create an HTTP client for the test server
        }

        [Theory]
        [InlineData("buy", "2,3")]
        [InlineData("sell", "5,3")]
        public async Task BuyAndSell_ShouldReturnOk(string type, string amount)
        {
            decimal amountConverted = Convert.ToDecimal(amount);
            var response = await ExecuteEndpointCall(type, amountConverted);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseDataString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<JsonDataResult<ExchangeOrderDTO>>(responseDataString);

            Assert.NotNull(responseData);
            Assert.Equal(amountConverted, responseData.Data.TotalAmount); 
        }

        [Theory]
        [InlineData("test", "1,3")]
        [InlineData("wrong", "5,3")]
        public async Task WrongType_ShouldReturnBadRequest(string type, string amount)
        {
            decimal amountConverted = Convert.ToDecimal(amount);
            var response = await ExecuteEndpointCall(type, amountConverted);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseDataString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<JsonDataResult<ExchangeOrderDTO>>(responseDataString);

            Assert.NotNull(responseData);
            Assert.Equal("Invlaid exchange type.", responseData.Errors[0]);
        }


        [Theory]
        [InlineData("buy", "0")]
        [InlineData("sell", "-5")]
        public async Task WrongAmount_ShouldReturnBadRequest(string type, string amount)
        {
            decimal amountConverted = Convert.ToDecimal(amount);
            var response = await ExecuteEndpointCall(type, amountConverted);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseDataString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<JsonDataResult<ExchangeOrderDTO>>(responseDataString);

            Assert.NotNull(responseData);
            Assert.Equal("Invalid amount.", responseData.Errors[0]);
        }

        [Theory]
        [InlineData("buy", "20000")]
        [InlineData("sell", "50000")]
        public async Task InsuficientAmount_ShouldReturnBadRequest(string type, string amount)
        {
            decimal amountConverted = Convert.ToDecimal(amount);
            var response = await ExecuteEndpointCall(type, amountConverted);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseDataString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<JsonDataResult<ExchangeOrderDTO>>(responseDataString);

            Assert.NotNull(responseData);
            Assert.Equal("Insufficient amount available to fulfill the target amount.", responseData.Errors[0]);
        }

        [Fact]
        public async Task NullAmount_ShouldReturnBadRequest()
        {
            var amount = new AmountNullDTO()
            {
                Amount = null
            };

            var jsonContent = JsonConvert.SerializeObject(amount);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var requestUri = $"http://localhost:5000/exchange/type/buy";
            var response = await _client.PostAsync(requestUri, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        public async Task<HttpResponseMessage> ExecuteEndpointCall(string type, decimal amount)
        {
            AmountDTO amountDTO = new AmountDTO();
            amountDTO.Amount = amount;

            var jsonContent = JsonConvert.SerializeObject(amountDTO);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var requestUri = $"http://localhost:5000/exchange/type/{type}";
            var response = await _client.PostAsync(requestUri, content);

            return response;
        }
    }
}

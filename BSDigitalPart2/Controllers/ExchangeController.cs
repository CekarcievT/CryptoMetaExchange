using BSDigitalPart2.DTOs;
using BSDigitalPart2.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Interfaces;
using Shared.Models;

namespace BSDigitalPart2.Controllers
{
    [ApiController]
    [ApiExceptionFilter]
    [Route("exchange")]
    public class ExchangeController : ControllerBase
    {
        public const int numberOfOrderBooks = 10;
        private readonly IPriceEvaluationService _priceEvaluationService;
        public ExchangeController(IPriceEvaluationService priceEvaluationService)
        {
            _priceEvaluationService = priceEvaluationService;
        }

        [HttpGet("type/{type}/amount/{amount}")]
        public async Task<IActionResult> UpdateChallenges(string type, decimal amount)
        {
            ExchangeOrderDTO result = new ExchangeOrderDTO();

            OrderType orderType = OrderType.Buy;
            if (type.ToLower() == "sell")
            {
                orderType = OrderType.Sell;
            } 
            else if (type.ToLower() == "buy")
            {
                orderType = OrderType.Buy;
            }
            else
            {
                return JsonDataResult<ExchangeOrderDTO>.MapResponse(
                     false,
                     null,
                     new List<string> {"Invlaid exchange type."},
                     StatusCodes.Status400BadRequest
                 );
            }

            List<ExchangeOrderBook> exchangeOrderBooks = _priceEvaluationService.ReadOrderBooksFromFile(numberOfOrderBooks);

            List<ExchangeOrder> bestPriceOrders = _priceEvaluationService.CalculateBestOrderPrice(exchangeOrderBooks, amount, orderType);

            result.Orders = bestPriceOrders;

            result.TotalPrice = bestPriceOrders.Sum(order => order.Price * order.Amount);
            result.TotalAmout = bestPriceOrders.Sum(order => order.Amount);

            return JsonDataResult<ExchangeOrderDTO>.MapResponse(
                  true,
                  result,
                  null,
                  StatusCodes.Status200OK
              );
        }
    }
}

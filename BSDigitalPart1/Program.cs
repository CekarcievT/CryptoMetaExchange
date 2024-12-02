﻿using Microsoft.Extensions.DependencyInjection;
using Shared.Enums;
using Shared.Helpers;
using Shared.Interfaces;
using Shared.Models;
using Shared.Services;

namespace BSDigitalPart1
{
    internal class Program
    {
        public const decimal amountToBuy = 2.0m;
        public const decimal amountToSell = 3.0m;
        public const int numberOfOrderBooks = 10;
        static void Main(string[] args)
        {
            try
            {
                var serviceProvider = new ServiceCollection()
               .AddSingleton<IPriceEvaluationService, PriceEvaluationService>()
               .BuildServiceProvider();  

                var priceEvaluationService = serviceProvider.GetRequiredService<IPriceEvaluationService>();
                List<ExchangeOrderBook> exchangeOrderBooks = priceEvaluationService.ReadOrderBooksFromFile(numberOfOrderBooks);

                List<ExchangeOrder> bestBuy = priceEvaluationService.CalculateBestOrderPrice(exchangeOrderBooks, amountToBuy, OrderType.Buy);
                List<ExchangeOrder> bestSell = priceEvaluationService.CalculateBestOrderPrice(exchangeOrderBooks, amountToSell, OrderType.Sell);

                Console.WriteLine("Best buy");
                foreach (var order in bestBuy)
                {
                    Console.WriteLine($"Exchange name: {order.ExchangeName} Order amount: {order.Amount} Order price {order.Price}");
                }

                Console.WriteLine($"Target amount: {amountToBuy} Amount {bestBuy.Sum(order => order.Amount)} Total price: {bestBuy.Sum(order => order.Price * order.Amount)}");

                Console.WriteLine(Environment.NewLine);

                Console.WriteLine("Best sell");
                foreach (var order in bestSell)
                {
                    Console.WriteLine($"Exchange name: {order.ExchangeName} Order amount: {order.Amount} Order price {order.Price}");
                }

                Console.WriteLine($"Target amount: {amountToSell} Amount {bestSell.Sum(order => order.Amount)} Total price: {bestSell.Sum(order => order.Price * order.Amount)}");
            }
            catch (Exception ex)
            {
                var errorCodes = EnumHelper.GetKeyValuePairsFromEnum<ErrorCodes>();
                if (errorCodes.Any(error => error.name == ex.Message))
                {
                    Console.WriteLine(EnumHelper.GetDescription<ErrorCodes>(ex.Message));
                }
                else
                {
                    Console.WriteLine($"Unexpected error happedened {ex.Message}");
                }
            }
        }
    }
}

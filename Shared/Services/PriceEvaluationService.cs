using Newtonsoft.Json;
using Shared.Enums;
using Shared.Helpers;
using Shared.Interfaces;
using Shared.Models;

namespace Shared.Services
{
    public class PriceEvaluationService: IPriceEvaluationService
    {
        public List<ExchangeOrder> CalculateBestOrderPrice(List<ExchangeOrderBook> exchangeOrderBooks, decimal targetAmount, OrderType orderType)
        {
            List<ExchangeOrder> allOrders = new List<ExchangeOrder>();
            if (orderType == OrderType.Buy)
            {
                allOrders = exchangeOrderBooks
                .Where(orderBook => orderBook.Bids != null && orderBook.Bids.Any())
                .SelectMany(orderBook => orderBook.Bids, (orderBook, order) => new ExchangeOrder
                {
                    Id = order.Order.Id,
                    Time = order.Order.Time,
                    Type = order.Order.Type,
                    Kind = order.Order.Kind,
                    Amount = order.Order.Amount,
                    Price = order.Order.Price,
                    ExchangeName = orderBook.ExchangeName
                })
                .OrderBy(order => order.Price)
                .ToList();
            }
            else
            {
                allOrders = exchangeOrderBooks
                   .SelectMany(orderBook => orderBook.Asks, (orderBook, order) => new ExchangeOrder
                   {
                       Id = order.Order.Id,
                       Time = order.Order.Time,
                       Type = order.Order.Type,
                       Kind = order.Order.Kind,
                       Amount = order.Order.Amount,
                       Price = order.Order.Price,
                       ExchangeName = orderBook.ExchangeName
                   })
                  .OrderByDescending(order => order.Price)
                  .ToList();
            }

            List<ExchangeOrder> lowestBuyOrders = new List<ExchangeOrder>();
            decimal accumulatedAmount = 0;

            foreach (var order in allOrders)
            {
                if (accumulatedAmount >= targetAmount)
                    break;

                decimal remainingAmount = targetAmount - accumulatedAmount;

                decimal amountToTake = Math.Min(order.Amount, remainingAmount);
                order.Amount = amountToTake;

                lowestBuyOrders.Add(order);

                accumulatedAmount += amountToTake;
            }

            return lowestBuyOrders;
        }

        // Calculate the highest sell orders
        public List<ExchangeOrder> CalculateHighestSell(List<ExchangeOrderBook> exchangeOrderBooks, decimal targetAmount)
        {
            List<ExchangeOrder> highestSellOrders = new List<ExchangeOrder>();
            decimal accumulatedAmount = 0;

            List<ExchangeOrder> allSellOrders = exchangeOrderBooks
                 .SelectMany(orderBook => orderBook.Asks, (orderBook, order) => new ExchangeOrder
                 {
                     Id = order.Order.Id,
                     Time = order.Order.Time,
                     Type = order.Order.Type,
                     Kind = order.Order.Kind,
                     Amount = order.Order.Amount,
                     Price = order.Order.Price,
                     ExchangeName = orderBook.ExchangeName
                 })
                .OrderByDescending(order => order.Price)
                .ToList();

            foreach (var order in allSellOrders)
            {
                if (accumulatedAmount >= targetAmount)
                    break;

                decimal remainingAmount = targetAmount - accumulatedAmount;

                decimal amountToTake = Math.Min(order.Amount, remainingAmount);
                order.Amount = amountToTake;

                highestSellOrders.Add(order);
                accumulatedAmount += amountToTake;
            }

            return highestSellOrders;
        }

        public List<ExchangeOrderBook> ReadOrderBooksFromFile(int numberOfLines)
        {
            List<ExchangeOrderBook> exchangeOrderBooks = new List<ExchangeOrderBook>();

            string contentRootPath = Directory.GetCurrentDirectory();
            string orderBooksFilePath = Path.Combine(contentRootPath, "Data", "order_books_data");

            if (File.Exists(orderBooksFilePath))
            {
                var lines = File.ReadLines(orderBooksFilePath);
                int linesRead = 0;

                foreach (var line in lines)
                {
                    if (linesRead >= numberOfLines)
                        break;

                    var parts = line.Split(new[] { '\t', ' ' }, 2);

                    if (parts.Length > 1)
                    {
                        string jsonPart = parts.LastOrDefault();

                        OrderBook orderBook = JsonConvert.DeserializeObject<OrderBook>(jsonPart);
                        ExchangeOrderBook exchangeOrderBook = new ExchangeOrderBook();

                        CopyHelper.CopyProperties<OrderBook>(orderBook, exchangeOrderBook);
                        exchangeOrderBook.ExchangeName = $"Exchange {linesRead + 1}";

                        if (orderBook != null)
                        {
                            exchangeOrderBooks.Add(exchangeOrderBook);
                        }
                    }

                    linesRead++;
                }

                return exchangeOrderBooks;
            }
            else
            {
                throw new Exception("File doesn't exist.");
            }
        }
    }
}

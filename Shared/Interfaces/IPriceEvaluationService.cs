using Shared.Enums;
using Shared.Models;

namespace Shared.Interfaces
{
    public interface IPriceEvaluationService
    {
        public List<ExchangeOrder> CalculateBestOrderPrice(List<ExchangeOrderBook> exchangeOrderBooks, decimal targetAmount, OrderType orderType);
        public List<ExchangeOrderBook> ReadOrderBooksFromFile(int numberOfLines);
    }
}

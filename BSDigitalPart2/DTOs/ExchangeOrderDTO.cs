using Shared.Models;

namespace BSDigitalPart2.DTOs
{
    public class ExchangeOrderDTO
    {
        public List<ExchangeOrder> Orders { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

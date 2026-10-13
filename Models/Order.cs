
using System.ComponentModel.DataAnnotations;

namespace BiteBudget.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string DeliveryAddress { get; set; } = string.Empty;

        public decimal Subtotal { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal Total { get; set; }

        public string PaymentMethod { get; set; } = "Pay on Delivery";
        public string PaymentStatus { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<OrderItem> Items { get; set; } = new();
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MealId { get; set; }
        public string MealName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; } = null!;
    }
}


namespace BiteBudget.Models
{
    public class CartLineViewModel
    {
        public Meal Meal { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal LineTotal => Meal.Price * Quantity;
    }
}

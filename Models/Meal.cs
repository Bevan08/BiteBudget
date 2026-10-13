
namespace BiteBudget.Models
{
    public class Meal
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Calories { get; set; }

        public int Protein { get; set; }

        public string Category { get; set; } = string.Empty;

        public bool IsVegetarian { get; set; }
    }
}

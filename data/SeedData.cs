
using BiteBudget.Models;

namespace BiteBudget.Data
{
    public static class SeedData
    {
        public static void Initialize(BiteBudgetContext context)
        {
            if (context.Meals.Any())
            {
                return;
            }

            var meals = new List<Meal>
            {
                new Meal
                {
                    Name = "Paneer Rice Bowl",
                    Description = "Paneer, rice and fresh vegetables",
                    Price = 120,
                    Calories = 450,
                    Protein = 20,
                    Category = "Lunch",
                    IsVegetarian = true
                },
                new Meal
                {
                    Name = "Grilled Chicken Bowl",
                    Description = "Grilled chicken with rice and vegetables",
                    Price = 180,
                    Calories = 520,
                    Protein = 38,
                    Category = "Lunch",
                    IsVegetarian = false
                },
                new Meal
                {
                    Name = "Veg Sandwich",
                    Description = "Whole wheat bread with fresh vegetables",
                    Price = 70,
                    Calories = 280,
                    Protein = 10,
                    Category = "Snack",
                    IsVegetarian = true
                },
                new Meal
                {
                    Name = "Egg Bhurji Meal",
                    Description = "Scrambled eggs served with chapati",
                    Price = 100,
                    Calories = 390,
                    Protein = 22,
                    Category = "Dinner",
                    IsVegetarian = false
                },
                new Meal
                {
                    Name = "Dal Khichdi",
                    Description = "Comforting rice and lentil meal",
                    Price = 80,
                    Calories = 350,
                    Protein = 12,
                    Category = "Dinner",
                    IsVegetarian = true
                }
            };

            context.Meals.AddRange(meals);
            context.SaveChanges();
        }
    }
}

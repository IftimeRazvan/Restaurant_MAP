using Restaurant.Models.DataAccessLayer;
using Restaurant.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.BusinessLogicLayer
{
    public class DishBL
    {
        private readonly DishDAL dishDAL = new DishDAL();
        private readonly AllergenDAL allergenDAL = new AllergenDAL();
        private readonly DishAllergenDAL dishAllergenDAL = new DishAllergenDAL();

        public List<Dish> GetDishesByCategory(string categoryName)
        {
            return dishDAL.GetDishesByCategory(categoryName);
        }

        public List<Dish> SearchDishesByName(string keyword,bool include)
        {
            return dishDAL.SearchByName(keyword,include);
        }

        public List<Dish> SearchDishesByAllergen(string keyword, bool include)
        {
            return dishDAL.SearchByAllergen(keyword, include);
        }

        public List<Dish> GetAllDishes()
        {
            return dishDAL.GetAllDishes();
        }

        public decimal GetTotalQuantity(int dishID)
        {
            return dishDAL.GetDishTotalQuantity(dishID);
        }

        public Dish GetDishByID(int dishID)
        {
            return dishDAL.GetDishById(dishID);
        }

        public void UpdateDish(Dish dish)
        {
            dishDAL.UpdateDish(dish);
        }

        public void InsertDish(Dish dish)
        {
            dishDAL.InsertDish(dish);
        }

        public void DeleteDish(int dishID)
        {
            dishDAL.DeleteDish(dishID);
        }

        public void SetDishAllergens(int dishId, IEnumerable<string> allergenNames)
        {
            dishAllergenDAL.DeleteByDish(dishId);

            var allAllergens = allergenDAL.GetAllAllergens();
            foreach (var name in allergenNames.Distinct())
            {
                var existing = allAllergens.FirstOrDefault(a =>
                    a.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));

                int allergenId;
                if (existing != null)
                {
                    allergenId = existing.AllergenID;
                }
                else
                {
                    allergenDAL.InsertAllergen(name);
                    allAllergens = allergenDAL.GetAllAllergens();
                    allergenId = allAllergens
                        .First(a => a.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
                        .AllergenID;
                }

                dishAllergenDAL.Insert(dishId, allergenId);
            }
        }

        public List<Dish> GetLowStockDishes(int threshold)
        {
            return dishDAL.GetLowStockDishes(threshold);
        }

    }
}

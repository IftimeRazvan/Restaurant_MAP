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
    }
}

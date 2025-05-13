using Restaurant.Models.DataAccessLayer;
using Restaurant.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.BusinessLogicLayer
{
    public class CategoryBL
    {
        private readonly CategoryDAL categoryDAL = new CategoryDAL();

        public List<Category> GetAllCategories()
        {
            return categoryDAL.GetAllCategories();
        }
    }
}

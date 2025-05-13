using Restaurant.Models.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Models.EntityLayer;

namespace Restaurant.Models.BusinessLogicLayer
{
    public class MenuBL
    {
        private readonly MenuDAL menuDAL = new MenuDAL();

        public List<Menu> GetMenusByCategory(string categoryName)
        {
            return menuDAL.GetMenusByCategory(categoryName);
        }
    }
}

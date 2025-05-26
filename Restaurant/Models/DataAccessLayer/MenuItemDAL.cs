using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Restaurant.Models.EntityLayer;
using System.Collections.ObjectModel;

namespace Restaurant.Models.DataAccessLayer
{
    public class MenuItemDAL
    {
        public ObservableCollection<MenuItem> GetMenuItemsForMenu(int menuId)
        {
            var items = new ObservableCollection<MenuItem>();

            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetMenuItemsForMenu", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@MenuId", SqlDbType.Int);
                param.Value = menuId;
                cmd.Parameters.Add(param);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var quantity = (decimal)reader["Quantity"];
                    var menuItem= new MenuItem
                    {
                        MenuItemID = (int)reader["MenuItemId"],
                        MenuID = (int?)reader["MenuId"],
                        DishID = (int)reader["DishId"],
                        Quantity = quantity,
                        Dish = new Dish
                        {
                            Name = reader["Name"].ToString(),
                            Price = (decimal)reader["Price"],
                            QuantityPerPortion = quantity
                        }

                    };
                    menuItem.Dish.DishID = menuItem.DishID;

                    items.Add(menuItem);
                }


                conn.Close();
            }

            return items;
        }
    }
}

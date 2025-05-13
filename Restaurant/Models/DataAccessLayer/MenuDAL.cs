using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Restaurant.Models.BusinessLogicLayer;
using Restaurant.Models.EntityLayer;

namespace Restaurant.Models.DataAccessLayer
{
    public class MenuDAL
    {
        public List<Menu> GetMenusByCategory(string categoryName)
        {
            var menus = new List<Menu>();

            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetMenusByCategoryWithDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@CategoryName", SqlDbType.NVarChar, 100);
                param.Value = categoryName;
                cmd.Parameters.Add(param);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    menus.Add(new Menu
                    {
                        MenuID = (int)reader["MenuId"],
                        Name = reader["Name"].ToString(),
                        ImageUrl = reader["ImageUrl"] as string,
                        CategoryID = (int)reader["CategoryId"],
                        Items = new List<MenuItem>(),
                        Allergens = new List<string>()
                    });
                }

                reader.NextResult();

                while (reader.Read()) 
                {
                    var menuItemId = (int)reader["MenuItemId"];
                    var menuId = (int?)reader["MenuId"];
                    var dishId = (int?)reader["DishId"];
                    var quantity = (decimal)reader["Quantity"];

                    var menu = menus.FirstOrDefault(m => m.MenuID == menuId);
                    if (menu != null)
                    {
                        menu.Items.Add(new MenuItem 
                        {
                            MenuItemID = menuItemId,
                            MenuID = menuId,
                            DishID = dishId,
                            Dish = new Dish
                            {
                                Name = reader["Name"].ToString(),
                                Price = (decimal)reader["Price"],
                                QuantityPerPortion =quantity
                            }
                        });
                    }
                }

                reader.NextResult();

                while (reader.Read())
                {
                    var menuId = (int)reader["MenuId"];
                    var allergenName = reader["AllergenName"].ToString();

                    var menu = menus.FirstOrDefault(m => m.MenuID == menuId);
                    if (menu != null && !string.IsNullOrEmpty(allergenName))
                    {
                        menu.Allergens.Add(allergenName);
                    }
                }



                conn.Close();
            }

            return menus;
        }

        public List<Menu> SearchByName(string keyword, bool include)
        {
            var menus = new List<Menu>();

            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SearchMenusByName", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@Keyword", SqlDbType.NVarChar, 100);
                param1.Value = keyword;
                cmd.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@Include", SqlDbType.Bit);
                param2.Value = include;
                cmd.Parameters.Add(param2);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var menu = new Menu
                    {
                        MenuID = (int)reader["MenuId"],
                        Name = reader["Name"].ToString(),
                        ImageUrl = reader["ImageUrl"] as string,
                        CategoryID = (int)reader["CategoryId"],
                        Items = new List<MenuItem>(),
                        Allergens = new List<string>()
                    };

                    menu.Items = new MenuItemDAL().GetMenuItemsForMenu(menu.MenuID);
                    menu.Allergens = new AllergenDAL().GetAllergensByMenu(menu.MenuID);
                    menus.Add(menu);
                }

                conn.Close();

            }
            return menus;
        }

        public List<Menu> SearchByAllergen(string keyword, bool include)
        {
            var menus = new List<Menu>();

            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SearchMenusByAllergen", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param1 = new SqlParameter("@Keyword", SqlDbType.NVarChar, 100);
                param1.Value = keyword;
                cmd.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@Include", SqlDbType.Bit);
                param2.Value = include;
                cmd.Parameters.Add(param2);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var menu = new Menu
                    {
                        MenuID = (int)reader["MenuId"],
                        Name = reader["Name"].ToString(),
                        ImageUrl = reader["ImageUrl"] as string,
                        CategoryID = (int)reader["CategoryId"],
                        Items = new List<MenuItem>(),
                        Allergens = new List<string>()
                    };
                    menu.Items = new MenuItemDAL().GetMenuItemsForMenu(menu.MenuID);
                    menu.Allergens = new AllergenDAL().GetAllergensByMenu(menu.MenuID);
                    menus.Add(menu);
                }

                conn.Close();
            }

            return menus;
        }
    }
}

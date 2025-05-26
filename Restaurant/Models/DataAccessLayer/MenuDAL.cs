using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Restaurant.Models.BusinessLogicLayer;
using Restaurant.Models.EntityLayer;


namespace Restaurant.Models.DataAccessLayer
{
    public class MenuDAL
    {
        public bool IsMenuAvailable(int menuID)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("MenuAvailable", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MenuId", menuID);

                // Adăugăm parametru pentru valoarea returnată
                var returnValue = new SqlParameter
                {
                    Direction = ParameterDirection.ReturnValue,
                    SqlDbType = SqlDbType.Int
                };
                cmd.Parameters.Add(returnValue);

                conn.Open();
                cmd.ExecuteNonQuery();

                int result = (int)returnValue.Value;
                return result == 1;
            }
        }

        public decimal GetMenuPrice(int menuId)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetMenuPrice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MenuId", menuId);

                conn.Open();
                var result = cmd.ExecuteScalar();

                return result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
            }
        }


        public List<Menu> GetAllMenus()
        {
            var menus = new List<Menu>();

            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetAllMenus", conn);
                cmd.CommandType = CommandType.StoredProcedure;

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
                        Items = new ObservableCollection<MenuItem>(),
                        Allergens = new List<string>()
                    };

                    menu.Items = new MenuItemDAL().GetMenuItemsForMenu(menu.MenuID);
                    menu.Allergens = new AllergenDAL().GetAllergensByMenu(menu.MenuID);
                    menu.IsAvailable = IsMenuAvailable(menu.MenuID);
                    decimal total = GetMenuPrice(menu.MenuID);
                    decimal discountPercentage = SettingsHelper.Discount_Menu_Percentage;
                    menu.CalculatedPrice = total - (total * discountPercentage / 100);
                    menus.Add(menu);
                }
                conn.Close();
            }

            return menus;
        }
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
                    var menu = new Menu
                    {
                        MenuID = (int)reader["MenuId"],
                        Name = reader["Name"].ToString(),
                        ImageUrl = reader["ImageUrl"] as string,
                        CategoryID = (int)reader["CategoryId"],
                        Items = new ObservableCollection<MenuItem>(),
                        Allergens = new List<string>()
                    };

                    menu.Items = new MenuItemDAL().GetMenuItemsForMenu(menu.MenuID);
                    menu.Allergens = new AllergenDAL().GetAllergensByMenu(menu.MenuID);
                    menu.IsAvailable = IsMenuAvailable(menu.MenuID);
                    decimal total = GetMenuPrice(menu.MenuID);
                    decimal discountPercentage = SettingsHelper.Discount_Menu_Percentage;
                    menu.CalculatedPrice = total - (total * discountPercentage / 100);
                    menus.Add(menu);
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
                        Items = new ObservableCollection<MenuItem>(),
                        Allergens = new List<string>()
                    };

                    menu.Items = new MenuItemDAL().GetMenuItemsForMenu(menu.MenuID);
                    menu.Allergens = new AllergenDAL().GetAllergensByMenu(menu.MenuID);
                    menu.IsAvailable = IsMenuAvailable(menu.MenuID);
                    decimal total = GetMenuPrice(menu.MenuID);
                    decimal discountPercentage = SettingsHelper.Discount_Menu_Percentage;
                    menu.CalculatedPrice = total - (total * discountPercentage / 100);
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
                        Items = new ObservableCollection<MenuItem>(),
                        Allergens = new List<string>()
                    };
                    menu.Items = new MenuItemDAL().GetMenuItemsForMenu(menu.MenuID);
                    menu.Allergens = new AllergenDAL().GetAllergensByMenu(menu.MenuID);
                    menu.IsAvailable = IsMenuAvailable(menu.MenuID);
                    decimal total = GetMenuPrice(menu.MenuID);
                    decimal discountPercentage = SettingsHelper.Discount_Menu_Percentage;
                    menu.CalculatedPrice = total - (total * discountPercentage / 100);
                    menus.Add(menu);
                }

                conn.Close();
            }

            return menus;
        }

        public int InsertMenu(Menu menu)
        {
            int menuId;

            using (SqlConnection conn = DALHelper.Connection)
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("InsertMenu", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", menu.Name);
                    cmd.Parameters.AddWithValue("@CategoryId", menu.CategoryID);

                    SqlParameter Id = new SqlParameter("@MenuId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(Id);

                    cmd.ExecuteNonQuery();
                    menuId = (int)Id.Value;
                }

                foreach (var mi in menu.Items)
                {
                    using (SqlCommand cmd = new SqlCommand("InsertMenuItem", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MenuId", menuId);
                        cmd.Parameters.AddWithValue("@DishId", mi.Dish.DishID);
                        cmd.Parameters.AddWithValue("@Quantity", mi.Quantity);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            return menuId;
        }

        public void UpdateMenu(Menu menu)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                conn.Open();

                // 1) update header
                using (SqlCommand cmd = new SqlCommand("UpdateMenu", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MenuId", menu.MenuID);
                    cmd.Parameters.AddWithValue("@Name", menu.Name);
                    cmd.Parameters.AddWithValue("@CategoryId", menu.CategoryID);
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand("DeleteMenuItemsByMenu", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MenuId", menu.MenuID);
                    cmd.ExecuteNonQuery();
                }

                foreach (var mi in menu.Items)
                {
                    using (SqlCommand cmd = new SqlCommand("InsertMenuItem", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MenuId", menu.MenuID);
                        cmd.Parameters.AddWithValue("@DishId", mi.Dish.DishID);
                        cmd.Parameters.AddWithValue("@Quantity", mi.Quantity);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }



        public void DeleteMenu(int menuId)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("DeleteMenu", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MenuId", menuId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}



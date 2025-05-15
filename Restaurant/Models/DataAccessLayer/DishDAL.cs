using Restaurant.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;

namespace Restaurant.Models.DataAccessLayer
{
    public class DishDAL
    {
        public List<Dish> GetAllDishes()
        {
            var dishes = new List<Dish>();

            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetAllDishes", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var dish = new Dish
                    {
                        DishID = (int)reader["DishID"],
                        Name = reader["Name"].ToString(),
                        Price = (decimal)reader["Price"],
                        QuantityPerPortion = (decimal)reader["QuantityPerPortion"],
                        TotalQuantity = (decimal)reader["TotalQuantity"],
                        ImageUrl = reader["ImageUrl"] as string,
                        CategoryID = (int?)reader["CategoryId"],
                        Allergens = new List<string>()
                    };

                    dish.Allergens = new AllergenDAL().GetAllergensByDish(dish.DishID);
                    dishes.Add(dish);
                }

                conn.Close();
            }

            return dishes;
        }
        public List<Dish> GetDishesByCategory(string categoryName)
        {
            var dishes = new List<Dish>();

            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetDishesByCategory", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@CategoryName", SqlDbType.NVarChar, 100);
                param.Value = categoryName;
                cmd.Parameters.Add(param);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var dish = new Dish
                    {
                        DishID = (int)reader["DishID"],
                        Name = reader["Name"].ToString(),
                        Price = (decimal)reader["Price"],
                        QuantityPerPortion = (decimal)reader["QuantityPerPortion"],
                        TotalQuantity = (decimal)reader["TotalQuantity"],
                        ImageUrl = reader["ImageUrl"] as string,
                        CategoryID = (int?)reader["CategoryId"],
                        Allergens = new List<string>()
                    };

                    dish.Allergens = new AllergenDAL().GetAllergensByDish(dish.DishID);
                    dishes.Add(dish);
                }

                conn.Close();
            }

            return dishes;
        }

        public List<Dish> SearchByName(string keyword, bool include)
        {
            var dishes = new List<Dish>();

            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SearchDishesByName", conn);
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
                    var dish = new Dish
                    {
                        DishID = (int)reader["DishID"],
                        Name = reader["Name"].ToString(),
                        Price = (decimal)reader["Price"],
                        QuantityPerPortion = (decimal)reader["QuantityPerPortion"],
                        ImageUrl = reader["ImageUrl"] as string,
                        CategoryID = (int?)reader["CategoryId"],
                        Allergens = new List<string>()
                    };

                    dish.Allergens = new AllergenDAL().GetAllergensByDish(dish.DishID);
                    dishes.Add(dish);
                }

                conn.Close();

            }

            return dishes;
        }

        public List<Dish> SearchByAllergen(string keyword, bool include)
        {
            var dishes = new List<Dish>();

            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("SearchDishesByAllergen", conn);
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
                    var dish = new Dish
                    {
                        DishID = (int)reader["DishID"],
                        Name = reader["Name"].ToString(),
                        Price = (decimal)reader["Price"],
                        QuantityPerPortion = (decimal)reader["QuantityPerPortion"],
                        ImageUrl = reader["ImageUrl"] as string,
                        CategoryID = (int?)reader["CategoryId"],
                        Allergens = new List<string>()
                    };

                    dish.Allergens = new AllergenDAL().GetAllergensByDish(dish.DishID);
                    dishes.Add(dish);
                }

                conn.Close();

            }

            return dishes;
        }
    }
}

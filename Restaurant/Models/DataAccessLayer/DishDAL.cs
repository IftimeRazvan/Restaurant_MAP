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
using Restaurant.Models.BusinessLogicLayer;

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

        public decimal GetDishTotalQuantity(int dishId)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetDishTotalQuantityById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DishID", dishId);

                conn.Open();

                var result = cmd.ExecuteScalar();

                if (result != null && result is decimal quantity)
                {
                    return quantity;
                }

                return 0m;
            }
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

        public Dish GetDishById(int dishId)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetDishById", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DishID", dishId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Dish
                    {
                        DishID = (int)reader["DishID"],
                        Name = reader["Name"].ToString(),
                        Price = (decimal)reader["Price"],
                        QuantityPerPortion = (decimal)reader["QuantityPerPortion"],
                        TotalQuantity = (decimal)reader["TotalQuantity"]
                    };
                }

                return null;
            }
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

        public void InsertDish(Dish dish)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("InsertDish", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", dish.Name);
                cmd.Parameters.AddWithValue("@Price", dish.Price);
                cmd.Parameters.AddWithValue("@QuantityPerPortion", dish.QuantityPerPortion);
                cmd.Parameters.AddWithValue("@TotalQuantity", dish.TotalQuantity);
                cmd.Parameters.AddWithValue("@CategoryId", dish.CategoryID);

                var DishId = new SqlParameter("@DishId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(DishId);

                conn.Open();
                cmd.ExecuteNonQuery();

                dish.DishID = (int)DishId.Value;
            }
        }

        public void DeleteDish(int dishId)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("DeleteDish", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DishId", dishId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateDish(Dish dish)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("UpdateDish", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DishId", dish.DishID);
                cmd.Parameters.AddWithValue("@Name", dish.Name);
                cmd.Parameters.AddWithValue("@Price", dish.Price);
                cmd.Parameters.AddWithValue("@QuantityPerPortion", dish.QuantityPerPortion);
                cmd.Parameters.AddWithValue("@TotalQuantity", dish.TotalQuantity);
                cmd.Parameters.AddWithValue("@CategoryId", dish.CategoryID);


                conn.Open();
                cmd.ExecuteNonQuery();

            }
        }

        public List<Dish> GetLowStockDishes(int threshold)
        {
            var dishes = new List<Dish>();

            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetLowStockDishes", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Threshold", threshold);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dishes.Add(new Dish
                    {
                        DishID = (int)reader["DishID"],
                        Name = reader["Name"].ToString(),
                        Price = (decimal)reader["Price"],
                        QuantityPerPortion = (decimal)reader["QuantityPerPortion"],
                        TotalQuantity = (decimal)reader["TotalQuantity"]
                    });
                }

                reader.Close();
            }

            return dishes;
        }
    }
}


using Restaurant.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Restaurant.Models.DataAccessLayer
{
    public class DishDAL
    {
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
                    dishes.Add(new Dish
                    {
                        DishID = (int)reader["DishID"],
                        Name = reader["Name"].ToString(),
                        Price = (decimal)reader["Price"],
                        QuantityPerPortion = (decimal)reader["QuantityPerPortion"],
                        TotalQuantity = (decimal)reader["TotalQuantity"],
                        ImageUrl = reader["ImageUrl"] as string,
                        CategoryID = (int?)reader["CategoryId"],
                        Allergens = new List<string>()
                    });
                }

                reader.NextResult();

                while (reader.Read())
                {
                    var dishId = (int)reader["DishId"];
                    var allergenName = reader["AllergenName"].ToString();

                    var dish = dishes.FirstOrDefault(d => d.DishID == dishId);
                    if (dish != null && !string.IsNullOrEmpty(allergenName))
                    {
                        dish.Allergens.Add(allergenName);
                    }
                }

                conn.Close();
            }

            return dishes;
        }
    }
}

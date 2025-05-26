using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Restaurant.Models.DataAccessLayer
{
    public class DishAllergenDAL
    {
        public void DeleteByDish(int dishId)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("DeleteDishAllergensByDish", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DishId", dishId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Insert(int dishId, int allergenId)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("InsertDishAllergen", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DishId", dishId);
                cmd.Parameters.AddWithValue("@AllergenId", allergenId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}

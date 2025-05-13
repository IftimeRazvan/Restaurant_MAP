using Restaurant.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Restaurant.Models.DataAccessLayer
{
    public class AllergenDAL
    {
        public List<string> GetAllergensByDish(int dishId)
        {
            var allergens = new List<string>();

            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetAllergensByDish", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@DishId", SqlDbType.Int);
                param.Value = dishId;
                cmd.Parameters.Add(param);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    allergens.Add(reader["Name"].ToString());
                }


                conn.Close();
            }

            return allergens;
        }

        public List<string> GetAllergensByMenu(int menuId)
        {
            var allergens = new List<string>();

            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetAllergensByMenu", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@MenuId", SqlDbType.Int);
                param.Value = menuId;
                cmd.Parameters.Add(param);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    allergens.Add(reader["Name"].ToString());
                }


                conn.Close();
            }

            return allergens;
        }


    }
}


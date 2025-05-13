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
    public class CategoryDAL
    {
        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();

            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetAllCategories", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        Name = reader["Name"].ToString(),
                        CategoryID = (int)reader["CategoryId"]
                    });
                }

                conn.Close();
            }

            return categories;
        }
    }
}

using Restaurant.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Restaurant.Models.BusinessLogicLayer;

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

        public void InsertCategory(string name)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("InsertCategory", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", name);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCategory(int categoryId)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("DeleteCategory", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCategory(int categoryId, string newName)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("UpdateCategory", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@Name", newName);

                conn.Open();
                cmd.ExecuteNonQuery();

            }
        }
    }
}

using Restaurant.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Runtime.Intrinsics.Arm;
using System.Windows.Documents;

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

        public ObservableCollection<Allergen> GetAllAllergens()
        {
            var allergens = new ObservableCollection<Allergen>();

            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("GetAllAllergens", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    allergens.Add(new Allergen
                    {
                        AllergenID = (int)reader["AllergenId"],
                        Name = reader["Name"].ToString()
                    });

                }

                conn.Close();
            }

            return allergens;
        }

        public  void InsertAllergen(string name)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("InsertAllergen", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", name);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public  void DeleteAllergen(int allergenId)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("DeleteAllergen", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AllergenId", allergenId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public  void UpdateAllergen(int allergenId, string newName)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("UpdateAllergen", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AllergenId", allergenId);
                cmd.Parameters.AddWithValue("@Name", newName);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

    }
}


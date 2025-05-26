using Restaurant.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;
using Restaurant.Models.BusinessLogicLayer;

namespace Restaurant.Models.DataAccessLayer
{
    public class UserDAL
    {

        public User LoginUser(string email,string password)
        {
            using (SqlConnection conn = DALHelper.Connection)
            {
                SqlCommand cmd = new SqlCommand("LoginUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new User
                    {

                        UserID = (int)reader["UserId"],
                        Email = reader["Email"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        IsEmployee = (bool)reader["IsEmployee"],
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public string RegisterUser(string firstName,string lastName,string email,string phoneNumber, string deliveryAddress,string password)
        {
            string message=string.Empty;
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("RegisterUser", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PhoneNumber", phoneNumber );
                command.Parameters.AddWithValue("@DeliveryAddress", deliveryAddress);
                command.Parameters.AddWithValue("@Password", password);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    message = reader["Message"].ToString();
                }

                connection.Close();
            }
            return message;
        }
    }
}

using Restaurant.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace Restaurant.Models.DataAccessLayer
{
    public class OrderDAL
    {
        public void AddOrder(Order order)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("AddOrder", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@UserId", order.UserID);
                command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                command.Parameters.AddWithValue("@Status", order.Status ?? "Inregistrata");
                command.Parameters.AddWithValue("@OrderCode", order.OrderCode);

                if (order.EstimatedDeliveryTime.HasValue)
                    command.Parameters.AddWithValue("@EstimatedDeliveryTime", order.EstimatedDeliveryTime.Value);
                else
                    command.Parameters.AddWithValue("@EstimatedDeliveryTime", DBNull.Value);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void AddOrderDetail(OrderDetail detail)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("AddOrderDetail", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderId", detail.OrderID);
                command.Parameters.AddWithValue("@DishId", detail.DishID ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@MenuId", detail.MenuID ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Quantity", detail.Quantity);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public int GetLastOrderId()
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("SELECT MAX(OrderID) FROM Orders", connection);
                connection.Open();
                var result = command.ExecuteScalar();
                return result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        public void UpdateDishStock(int dishId, int quantity)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("UpdateDishStock", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@DishId", dishId);
                command.Parameters.AddWithValue("@Quantity", quantity);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public int GetRecentOrders(int userId, int days)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetRecentOrders", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Days", days);

                connection.Open();
                var result = command.ExecuteScalar();
                return result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("UpdateOrderStatus", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderId", orderId);
                command.Parameters.AddWithValue("@NewStatus", newStatus);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void RestoreDishStock(int dishId, int quantity)
        {
            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("RestoreDishStock", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@DishId", dishId);
                command.Parameters.AddWithValue("@Quantity", quantity);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            var orders = new List<Order>();

            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetOrdersByUserId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var order =new Order
                    {
                        OrderID = (int)reader["OrderID"],
                        UserID = (int)reader["UserId"],
                        OrderDate = (DateTime)reader["OrderDate"],
                        Status = reader["Status"].ToString(),
                        OrderCode = reader["OrderCode"].ToString(),
                        EstimatedDeliveryTime = reader["EstimatedDeliveryTime"] as DateTime?,
                    };
                    order.OrderDetails = GetOrderDetails(order.OrderID);
                    orders.Add(order);
                }

                reader.Close();
            }

            return orders;
        }

        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            var details = new List<OrderDetail>();

            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetOrderDetails", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderId", orderId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    details.Add(new OrderDetail
                    {
                        OrderDetailID = (int)reader["OrderDetailID"],
                        OrderID = (int)reader["OrderID"],
                        DishID = reader["DishID"] as int?,
                        MenuID = reader["MenuID"] as int?,
                        Quantity = (int)reader["Quantity"],
                        ItemName = reader["ItemName"] as string ?? "Produs necunoscut"
                    });
                }

                reader.Close();
            }

            return details;
        }

        public List<Order> GetAllOrders()
        {
            var orders = new List<Order>();

            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetAllOrders", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var order = new Order
                    {
                        OrderID = (int)reader["OrderID"],
                        UserID = (int)reader["UserID"],
                        OrderDate = (DateTime)reader["OrderDate"],
                        Status = reader["Status"].ToString(),
                        OrderCode = reader["OrderCode"].ToString(),
                        EstimatedDeliveryTime = reader["EstimatedDeliveryTime"] as DateTime?,
                    };
                    order.OrderDetails = GetOrderDetails(order.OrderID);
                    orders.Add(order);
                }

                reader.Close();
            }

            return orders;
        }

        public List<Order> GetActiveOrders()
        {
            var orders = new List<Order>();

            using (SqlConnection connection = DALHelper.Connection)
            {
                SqlCommand command = new SqlCommand("GetActiveOrders", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var order = new Order
                    {
                        OrderID = (int)reader["OrderID"],
                        UserID = (int)reader["UserID"],
                        OrderDate = (DateTime)reader["OrderDate"],
                        Status = reader["Status"].ToString(),
                        OrderCode = reader["OrderCode"].ToString(),
                        EstimatedDeliveryTime = reader["EstimatedDeliveryTime"] as DateTime?,
                    };
                    order.OrderDetails = GetOrderDetails(order.OrderID);
                    orders.Add(order);
                }

                reader.Close();
            }

            return orders;
        }

    }

}



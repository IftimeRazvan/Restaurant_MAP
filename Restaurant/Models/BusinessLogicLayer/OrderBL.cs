using Restaurant.Models.DataAccessLayer;
using Restaurant.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.BusinessLogicLayer
{
    public class OrderBL
    {
        private readonly OrderDAL orderDAL = new OrderDAL();
        public void PlaceOrder(Order order, List<ShoppingCartItem<object>> cartItems)
        {
            // Generează cod unic
            order.OrderCode = GenerateUniqueOrderCode();

            order.EstimatedDeliveryTime = DateTime.Now.AddMinutes(60);

            // Salvează comanda
            orderDAL.AddOrder(order);

            // Obține ID-ul ultimei comenzi
            int orderId = orderDAL.GetLastOrderId();

            // Salvează fiecare produs din coș
            foreach (var item in cartItems)
            {
                if (item.Item is Dish dish)
                {
                    var detail = new OrderDetail
                    {
                        OrderID = orderId,
                        DishID = dish.DishID,
                        Quantity = item.Quantity
                    };

                    orderDAL.AddOrderDetail(detail);
                    orderDAL.UpdateDishStock(dish.DishID, item.Quantity);
                }
                else if (item.Item is Menu menu)
                {
                    var detail = new OrderDetail
                    {
                        OrderID = orderId,
                        MenuID = menu.MenuID,
                        Quantity = item.Quantity
                    };

                    orderDAL.AddOrderDetail(detail);

                    foreach (var component in menu.Items)
                    {
                        int requiredQuantity = (int)component.Quantity * item.Quantity;

                        orderDAL.UpdateDishStock(component.DishID, requiredQuantity);
                    }
                }
            }
        }
        private string GenerateUniqueOrderCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(); // Ex: A1B2C3D4
        }

        public int GetRecentOrders(int userId, int days = 7)
        {
            return orderDAL.GetRecentOrders(userId, days);
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            return orderDAL.GetOrdersByUserId(userId);
        }

        public void CancelOrder(int orderId)
        {
            orderDAL.UpdateOrderStatus(orderId, "Anulata");
        }

        public void RestoreDishStock(int dishId, int quantity)
        {
            orderDAL.RestoreDishStock(dishId, quantity);
        }

        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            return orderDAL.GetOrderDetails(orderId);
        }

        public List<Order> GetAllOrders()
        {
            return orderDAL.GetAllOrders();
        }

        public List<Order> GetActiveOrders()
        {
            return orderDAL.GetActiveOrders();
        }
    }
}

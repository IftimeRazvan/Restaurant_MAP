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
            order.OrderCode = GenerateUniqueOrderCode();

            order.EstimatedDeliveryTime = DateTime.Now.AddMinutes(60);

            orderDAL.AddOrder(order);

            int orderId = orderDAL.GetLastOrderId();

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

        public decimal GetOrderBasePrice(int orderId)
        {
            return orderDAL.GetOrderBasePrice(orderId);
        }

        public decimal CalculateOrderTotal(int orderId)
        {
            decimal basePrice = GetOrderBasePrice(orderId);

            decimal finalPrice = ApplyDiscounts(basePrice, orderId);

            finalPrice = AddDeliveryCost(finalPrice);

            return finalPrice;
        }

        private decimal AddDeliveryCost(decimal price)
        {
            if (price < SettingsHelper.Free_Delivery_Threshold)
            {
                return price + SettingsHelper.Delivery_Cost;
            }

            return price;
        }

        private decimal ApplyDiscounts(decimal basePrice, int orderId)
        {
            if (basePrice >= SettingsHelper.Minimum_Order_For_Discount)
            {
                return basePrice * (1 - SettingsHelper.Discount_Percentage / 100);
            }

            return basePrice;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.EntityLayer
{
    public class Order : BasePropertyChanged
    {
        private int orderID;
        private int userID;
        private string orderCode;
        private DateTime orderDate;
        private DateTime? estimatedDeliveryTime;
        private string status;
        private decimal totalPrice;



        public decimal TotalPrice
        {
            get =>totalPrice;
            set
            {
                totalPrice = value;
                NotifyPropertyChanged();
            }
        }


        public int OrderID
        {
            get => orderID;
            set
            {
                orderID = value;
                NotifyPropertyChanged();
            }
        }

        public int UserID
        {
            get => userID;
            set
            {
                userID = value;
                NotifyPropertyChanged();
            }
        }

        public string OrderCode
        {
            get => orderCode;
            set
            {
                orderCode = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime OrderDate
        {
            get => orderDate;
            set
            {
                orderDate = value;
                NotifyPropertyChanged();
            }
        }

        private string _customerName;
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                NotifyPropertyChanged();
            }
        }

        private string _customerPhone;
        public string CustomerPhone
        {
            get { return _customerPhone; }
            set
            {
                _customerPhone = value;
                NotifyPropertyChanged();
            }
        }

        private string _deliveryAddress;
        public string DeliveryAddress
        {
            get { return _deliveryAddress; }
            set
            {
                _deliveryAddress = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime? EstimatedDeliveryTime
        {
            get => estimatedDeliveryTime;
            set
            {
                estimatedDeliveryTime = value;
                NotifyPropertyChanged();
            }
        }

        public string Status
        {
            get => status;
            set
            {
                status = value;
                NotifyPropertyChanged();
            }
        }

        private List<OrderDetail> orderDetails = new List<OrderDetail>();
        public List<OrderDetail> OrderDetails
        {
            get => orderDetails;
            set
            {
                orderDetails = value;
                NotifyPropertyChanged();
            }
        }

        public bool CanBeCancelled => Status == "Inregistrata" || Status == "Se pregateste" || Status == "a plecat la client";
    }
}

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
    }
}

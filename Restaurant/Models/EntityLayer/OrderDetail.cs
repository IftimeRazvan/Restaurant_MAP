using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.EntityLayer
{
    public class OrderDetail : BasePropertyChanged
    {
        private int orderDetailID;
        private int orderID;
        private int? dishID;
        private int? menuID;
        private int quantity;

        public int OrderDetailID
        {
            get => orderDetailID;
            set
            {
                orderDetailID = value;
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

        public int? DishID
        {
            get => dishID;
            set
            {
                dishID = value;
                NotifyPropertyChanged();
            }
        }

        public int? MenuID
        {
            get => menuID;
            set
            {
                menuID = value;
                NotifyPropertyChanged();
            }
        }

        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                NotifyPropertyChanged();
            }
        }
    }
}

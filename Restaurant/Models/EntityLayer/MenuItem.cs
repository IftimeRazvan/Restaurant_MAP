using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.EntityLayer
{
    public class MenuItem : BasePropertyChanged
    {
        private int menuItemID;
        private int? menuID;
        private int? dishID;
        private decimal quantity;

        public int MenuItemID
        {
            get => menuItemID;
            set
            {
                menuItemID = value;
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

        public int? DishID
        {
            get => dishID;
            set
            {
                dishID = value;
                NotifyPropertyChanged();
            }
        }

        public decimal Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                NotifyPropertyChanged();
            }
        }

        public Dish Dish { get; set; }
    }
}

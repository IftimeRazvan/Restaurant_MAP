using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.EntityLayer
{
    public class Menu : BasePropertyChanged
    {
        private int menuID;
        private string name;
        private string imageUrl;
        private int categoryID;
        private List<MenuItem> items = new List<MenuItem>();
        private List<string> allergens = new List<string>();

        public int MenuID
        {
            get => menuID;
            set
            {
                menuID = value;
                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        public string ImageUrl
        {
            get => imageUrl;
            set
            {
                imageUrl = value;
                NotifyPropertyChanged();
            }
        }

        public int CategoryID
        {
            get => categoryID;
            set
            {
                categoryID = value;
                NotifyPropertyChanged();
            }
        }

        public List<MenuItem> Items
        {
            get => items;
            set
            {
                items = value;
                NotifyPropertyChanged();
                //NotifyPropertyChanged(nameof(IsAvailable));
            }
        }

        public List<string> Allergens
        {
            get => allergens;
            set
            {
                allergens = value;
                NotifyPropertyChanged();
            }
        }

        public decimal CalculatedPrice => CalculatePrice();

        private decimal CalculatePrice()
        {
            decimal total = Items?.Sum(i => i.Dish?.Price) ?? 0;
            decimal discountPercentage = SettingsHelper.Discount_Menu_Percentage;
            return total - (total * discountPercentage / 100);
        }

        public string ComponentDetails
        {
            get
            {
                return string.Join(", ", Items.Select(i => $"{i.Dish?.Name} x{i.Dish.QuantityPerPortion}g"));
            }
        }

        public string AllergensString => string.Join(", ", Allergens);

        public bool IsAvailable
        {
            get
            {
                return Items.All(item => item.Dish != null && item.Dish.TotalQuantity >= item.Quantity);
            }
        }

    }
}

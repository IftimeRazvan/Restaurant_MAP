using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.EntityLayer
{
    public class Dish : BasePropertyChanged
    {
        private int dishID;
        private string name;
        private decimal price;
        private decimal quantityPerPortion;
        private decimal totalQuantity;
        private string imageUrl;
        private int? categoryID;
        private List<string> allergens;


        public int DishID
        {
            get => dishID;
            set
            {
                dishID = value;
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

        public decimal Price
        {
            get => price;
            set
            {
                price = value;
                NotifyPropertyChanged();
            }
        }

        public decimal QuantityPerPortion
        {
            get => quantityPerPortion;
            set
            {
                quantityPerPortion = value;
                NotifyPropertyChanged();
            }
        }

        public decimal TotalQuantity
        {
            get => totalQuantity;
            set
            {
                totalQuantity = value;
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

        public int? CategoryID
        {
            get => categoryID;
            set
            {
                categoryID = value;
                NotifyPropertyChanged();
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
        public string AllergensString => string.Join(", ", Allergens);
        public bool IsAvailable => TotalQuantity > SettingsHelper.Minimum_Stock_Alert;

    }
}

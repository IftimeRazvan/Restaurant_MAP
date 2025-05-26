using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<MenuItem> items = new ObservableCollection<MenuItem>();
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

        public ObservableCollection<MenuItem> Items
        {
            get => items;
            set
            {
                items = value;
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

        public decimal CalculatedPrice { get; set; }

        public string ComponentDetails
        {
            get
            {
                return string.Join(", ", Items.Select(i => $"{i.Dish?.Name} x{i.Dish.QuantityPerPortion}g"));
            }
        }

        public string AllergensString => string.Join(", ", Allergens);

        public bool IsAvailable { get; set; }
    }
}

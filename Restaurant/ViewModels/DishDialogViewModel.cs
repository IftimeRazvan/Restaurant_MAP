using Restaurant.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels
{
    public class DishDialogViewModel : BasePropertyChanged
    {
        public Dish Dish { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Allergen> Allergens { get; set; }

        public ObservableCollection<Allergen> SelectedAllergens { get; set; } = new();
        public Allergen? SelectedAllergen { get; set; }

        public DishDialogViewModel(Dish dish,ObservableCollection<Category> categories,ObservableCollection<Allergen> allergens)
        {
            Dish = dish;
            Categories = categories;
            Allergens = allergens;
        }

    }
}

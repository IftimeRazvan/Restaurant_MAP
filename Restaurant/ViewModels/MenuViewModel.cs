using Restaurant.Models.BusinessLogicLayer;
using Restaurant.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels
{
    public class MenuViewModel : BasePropertyChanged
    {
        private readonly DishBL dishBL = new DishBL();
        private readonly MenuBL menuBL = new MenuBL();
        private readonly CategoryBL categoryBL = new CategoryBL();

        private ObservableCollection<Dish> allDishes = new ObservableCollection<Dish>();
        private ObservableCollection<Menu> allMenus = new ObservableCollection<Menu>();
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        private Category selectedCategory;

        public ObservableCollection<Dish> AllDishes
        {
            get => allDishes;
            set
            {
                allDishes = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Menu> AllMenus
        {
            get => allMenus;
            set
            {
                allMenus = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Category> Categories
        {
            get => categories;
            set
            {
                categories = value;
                NotifyPropertyChanged();
            }
        }

        public Category SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                NotifyPropertyChanged();
                LoadDishesBySelectedCategory();
            }
        }

        public MenuViewModel()
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            var rawCategories = categoryBL.GetAllCategories();
            Categories = new ObservableCollection<Category>(rawCategories);
        }

        private void LoadDishesBySelectedCategory()
        {
            if (SelectedCategory == null) return;

            var filteredDishes = dishBL.GetDishesByCategory(SelectedCategory.Name);
            AllDishes = new ObservableCollection<Dish>(filteredDishes);

            var filteredMenus = menuBL.GetMenusByCategory(SelectedCategory.Name);
            AllMenus = new ObservableCollection<Menu>(filteredMenus);

        }
    }
}

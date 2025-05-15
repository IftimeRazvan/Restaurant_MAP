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
        private string searchQuery;
        private string allergenQuery;
        private bool includeSearchTerm = false;
        private bool includeAllergen = false;

        public ObservableCollection<Dish> AllDishes
        {
            get => allDishes;
            set
            {
                allDishes = value;
                NotifyPropertyChanged();
            }
        }

        public bool IncludeSearchTerm
        {
            get => includeSearchTerm;
            set
            {
                includeSearchTerm = value;
                NotifyPropertyChanged();
            }
        }

        public bool IncludeAllergen
        {
            get => includeAllergen;
            set
            {
                includeAllergen = value;
                NotifyPropertyChanged();
            }
        }

        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                searchQuery = value;
                NotifyPropertyChanged();
                LoadDishesAndMenusBySearchName();
            }
        }

        public string AllergenQuery
        {
            get => allergenQuery;
            set
            {
                allergenQuery = value;
                NotifyPropertyChanged();
                LoadDishesAndMenusBySearchAllergen();
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
                LoadDishesAndMenusBySelectedCategory();
            }
        }

        public MenuViewModel()
        {
            LoadCategories();
            LoadDishesAndMenus();
        }

        private void LoadCategories()
        {
            var rawCategories = categoryBL.GetAllCategories();
            Categories = new ObservableCollection<Category>(rawCategories);
        }

        private void LoadDishesAndMenus()
        {
            var allDishes = dishBL.GetAllDishes();
            AllDishes = new ObservableCollection<Dish>(allDishes);
            var allMenus = menuBL.GetAllMenus();
            AllMenus = new ObservableCollection<Menu>(allMenus);
        }

        private void LoadDishesAndMenusBySelectedCategory()
        {
            if (SelectedCategory == null) return;

            var filteredDishes = dishBL.GetDishesByCategory(SelectedCategory.Name);
            AllDishes = new ObservableCollection<Dish>(filteredDishes);

            var filteredMenus = menuBL.GetMenusByCategory(SelectedCategory.Name);
            AllMenus = new ObservableCollection<Menu>(filteredMenus);

        }

        private void LoadDishesAndMenusBySearchName() 
        {

            var filteredDishes = dishBL.SearchDishesByName(SearchQuery,IncludeSearchTerm);
            AllDishes = new ObservableCollection<Dish>(filteredDishes);

            var filteredMenus = menuBL.SearchMenusByName(SearchQuery, IncludeSearchTerm);
            AllMenus = new ObservableCollection<Menu>(filteredMenus);
        }

        private void LoadDishesAndMenusBySearchAllergen()
        {

            var filteredDishes = dishBL.SearchDishesByAllergen(AllergenQuery,IncludeAllergen);
            AllDishes = new ObservableCollection<Dish>(filteredDishes);

            var filteredMenus = menuBL.SearchMenusByAllergen(AllergenQuery, IncludeAllergen);
            AllMenus = new ObservableCollection<Menu>(filteredMenus);


        }
    }
}

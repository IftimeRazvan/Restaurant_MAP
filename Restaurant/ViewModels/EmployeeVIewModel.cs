using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using Restaurant.Models.DataAccessLayer;
using Restaurant.Models.EntityLayer;
using Restaurant.ViewModels.Commands;
using Restaurant.Models.BusinessLogicLayer;
using Restaurant.Views;
using System.Windows;

namespace Restaurant.ViewModels
{
    public class EmployeeVIewModel : BasePropertyChanged
    {
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public ObservableCollection<Dish> Dishes { get; set; } = new ObservableCollection<Dish>();
        public ObservableCollection<Menu> Menus { get; set; } = new ObservableCollection<Menu>();
        public ObservableCollection<Allergen> Allergens { get; set; } = new ObservableCollection<Allergen>();

        private Category? selectedCategory;
        public Category? SelectedCategory
        { 
            get => selectedCategory; 
            set 
            {
                selectedCategory = value;
                NotifyPropertyChanged(); 
            } 
        }
        private Dish? selectedDish;
        public Dish? SelectedDish
        {
            get => selectedDish; 
            set 
            {
                selectedDish = value;
                NotifyPropertyChanged(); 
            } 
        }
        private Menu? selectedMenu;
        public Menu? SelectedMenu
        { 
            get => selectedMenu; 
            set 
            {
                selectedMenu = value;
                NotifyPropertyChanged(); 
            } 
        }
        private Allergen? selectedAllergen;
        public Allergen? SelectedAllergen
        { 
            get => selectedAllergen;
            set 
            { 
                selectedAllergen = value;
                NotifyPropertyChanged(); 
            } 
        }
        

        private ICommand addCategoryCommand;
        public ICommand AddCategoryCommand
        {
            get
            {
                return addCategoryCommand ??= new RelayCommand(_ => AddCategory());
            }
        }

        private ICommand editCategoryCommand;
        public ICommand EditCategoryCommand
        {
            get
            {
                return editCategoryCommand ??= new RelayCommand(_ => EditCategory(), _ => SelectedCategory != null);
            }
        }

        private ICommand deleteCategoryCommand;
        public ICommand DeleteCategoryCommand
        {
            get
            {
                return deleteCategoryCommand ??= new RelayCommand(_ => DeleteCategory(), _ => SelectedCategory != null);
            }
        }

        private ICommand addDishCommand;
        public ICommand AddDishCommand
        {
            get
            {
                return addDishCommand ??= new RelayCommand(_ => AddDish());
            }
        }

        private ICommand editDishCommand;
        public ICommand EditDishCommand
        {
            get
            {
                return editDishCommand ??= new RelayCommand(_ => EditDish(), _ => SelectedDish != null);
            }
        }

        private ICommand deleteDishCommand;
        public ICommand DeleteDishCommand
        {
            get
            {
                return deleteDishCommand ??= new RelayCommand(_ => DeleteDish(), _ => SelectedDish != null);
            }
        }

        private ICommand addMenuCommand;
        public ICommand AddMenuCommand
        {
            get
            {
                return addMenuCommand ??= new RelayCommand(_ => AddMenu());
            }
        }

        private ICommand editMenuCommand;
        public ICommand EditMenuCommand
        {
            get
            {
                return editMenuCommand ??= new RelayCommand(_ => EditMenu(), _ => SelectedMenu != null);
            }
        }

        private ICommand deleteMenuCommand;
        public ICommand DeleteMenuCommand
        {
            get
            {
                return deleteMenuCommand ??= new RelayCommand(_ => DeleteMenu(), _ => SelectedMenu != null);
            }
        }

        private ICommand addAllergenCommand;
        public ICommand AddAllergenCommand
        {
            get
            {
                return addAllergenCommand ??= new RelayCommand(_ => AddAllergen());
            }
        }

        private ICommand editAllergenCommand;
        public ICommand EditAllergenCommand
        {
            get
            {
                return editAllergenCommand ??= new RelayCommand(_ => EditAllergen(), _ => SelectedAllergen != null);
            }
        }

        private ICommand deleteAllergenCommand;
        public ICommand DeleteAllergenCommand
        {
            get
            {
                return deleteAllergenCommand ??= new RelayCommand(_ => DeleteAllergen(), _ => SelectedAllergen != null);
            }
        }

        private readonly CategoryBL categoryBL = new();
        private readonly DishBL dishBL = new();
        private readonly MenuBL menuBL = new();
        private readonly AllergenBL allergenBL = new();
        private readonly OrderBL orderBL = new();

        public EmployeeVIewModel()
        {
            LoadCatalog();
            LoadLowStockDishes();
        }

        private void LoadCatalog()
        {
            Categories.Clear();
            foreach (var c in categoryBL.GetAllCategories()) Categories.Add(c);

            Dishes.Clear();
            foreach (var d in dishBL.GetAllDishes()) Dishes.Add(d);

            Allergens.Clear();
            foreach (var a in allergenBL.GetAllAllergens()) Allergens.Add(a);

            Menus.Clear();
            foreach (var m in menuBL.GetAllMenus())
            {
                var items = new MenuItemDAL().GetMenuItemsForMenu(m.MenuID);
                foreach (var mi in items) m.Items.Add(mi);
                Menus.Add(m);
            }

        }

        private void AddCategory()
        {
            var dialog = new InputDialog("New category name:");
            if (dialog.ShowDialog() != true) return;

            categoryBL.InsertCategory(dialog.Value);
            LoadCatalog();
        }

        private void EditCategory()
        {
            var dialog = new InputDialog("Edit category name:")
            {
                Value = SelectedCategory!.Name
            };
            if (dialog.ShowDialog() != true) return;

            categoryBL.UpdateCategory(SelectedCategory.CategoryID, dialog.Value);
            LoadCatalog();
        }

        private void DeleteCategory()
        {
            categoryBL.DeleteCategory(SelectedCategory.CategoryID);
            LoadCatalog();
        }

        private void AddDish()
        {
            var newDish = new Dish
            {
                CategoryID = Categories.FirstOrDefault()?.CategoryID ?? 0,
                QuantityPerPortion = 1,
                TotalQuantity = 0,
                Price = 0m
            };

            var vm = new DishDialogViewModel(newDish, Categories, Allergens);
            var dialog = new DishDialogView { DataContext = vm };

            if (dialog.ShowDialog() == true)
            {
                dishBL.InsertDish(newDish);
                dishBL.SetDishAllergens(newDish.DishID, newDish.Allergens);
                LoadCatalog();
            }
        }

        private void EditDish()
        {
            if (SelectedDish == null) return;

            var dishClone = new Dish
            {
                DishID = SelectedDish.DishID,
                Name = SelectedDish.Name,
                Price = SelectedDish.Price,
                QuantityPerPortion = SelectedDish.QuantityPerPortion,
                TotalQuantity = SelectedDish.TotalQuantity,
                CategoryID = SelectedDish.CategoryID
            };

            var vm = new DishDialogViewModel(dishClone, Categories, Allergens);
            var dialog = new DishDialogView { DataContext = vm };

            if (dialog.ShowDialog() == true)
            {
                dishBL.UpdateDish(dishClone);
                dishBL.SetDishAllergens(dishClone.DishID, dishClone.Allergens);
                LoadCatalog();
            }
        }

        private void DeleteDish()
        {
            dishBL.DeleteDish(SelectedDish.DishID);
            LoadCatalog();
            //LoadLowStock();
        }


        private void AddMenu()
        {
            var newMenu = new Menu
            {
                CategoryID = Categories.FirstOrDefault()?.CategoryID ?? 0
            };
            var vm = new MenuDialogViewModel(newMenu, Categories, Dishes);
            var dialog = new MenuDialogView { DataContext = vm };

            if (dialog.ShowDialog() == true)
            {
                menuBL.InsertMenu(newMenu);
                LoadCatalog();
            }
        }

        private void EditMenu()
        {
            if (SelectedMenu == null) return;

            SelectedMenu.Items.Clear();
            foreach (var mi in new MenuItemDAL().GetMenuItemsForMenu(SelectedMenu.MenuID))
                SelectedMenu.Items.Add(mi);

            var clone = new Menu
            {
                MenuID = SelectedMenu.MenuID,
                Name = SelectedMenu.Name,
                CategoryID = SelectedMenu.CategoryID
            };
            foreach (var mi in SelectedMenu.Items)
                clone.Items.Add(new MenuItem
                {
                    Dish = mi.Dish,
                    Quantity = mi.Quantity
                });

            var vm = new MenuDialogViewModel(clone, Categories, Dishes);
            var dialog = new MenuDialogView { DataContext = vm };

            if (dialog.ShowDialog() == true)
            {
                menuBL.UpdateMenu(clone);
                LoadCatalog();
            }
        }

        private void DeleteMenu()
        {
            menuBL.DeleteMenu(SelectedMenu.MenuID);
            LoadCatalog();
        }


        private void AddAllergen()
        {
            var dialog = new InputDialog("Allergen name:");
            if (dialog.ShowDialog() != true) return;

            allergenBL.InsertAllergen(dialog.Value);
            LoadCatalog();
        }

        private void EditAllergen()
        {
            var dialog = new InputDialog("Edit allergen:")
            {
                Value = SelectedAllergen!.Name
            };
            if (dialog.ShowDialog() != true) return;

            allergenBL.UpdateAllergen(SelectedAllergen.AllergenID, dialog.Value);
            LoadCatalog();
        }

        private void DeleteAllergen()
        {
            allergenBL.DeleteAllergen(SelectedAllergen.AllergenID);
            LoadCatalog();
        }

        private ObservableCollection<Order> displayedOrders = new ObservableCollection<Order>();
        public ObservableCollection<Order> DisplayedOrders
        {
            get => displayedOrders;
            set
            {
                displayedOrders = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Dish> lowStockDishes = new ObservableCollection<Dish>();
        public ObservableCollection<Dish> LowStockDishes
        {
            get => lowStockDishes;
            set
            {
                lowStockDishes = value;
                NotifyPropertyChanged();
            }
        }

        private ICommand loadAllOrdersCommand;
        public ICommand LoadAllOrdersCommand
        {
            get
            {
                if (loadAllOrdersCommand == null)
                    loadAllOrdersCommand = new RelayCommand(LoadAllOrders);
                return loadAllOrdersCommand;
            }
        }

        private void LoadAllOrders(object param = null)
        {
            var orders = orderBL.GetAllOrders();
            DisplayedOrders = new ObservableCollection<Order>(
                orders.OrderByDescending(o => o.OrderDate)
            );
        }

        private ICommand loadActiveOrdersCommand;
        public ICommand LoadActiveOrdersCommand
        {
            get
            {
                if (loadActiveOrdersCommand == null)
                    loadActiveOrdersCommand = new RelayCommand(LoadActiveOrders);
                return loadActiveOrdersCommand;
            }
        }

        private void LoadActiveOrders(object param = null)
        {
            var orders = orderBL.GetActiveOrders(); 
            DisplayedOrders = new ObservableCollection<Order>(
                orders.OrderByDescending(o => o.OrderDate)
            );
        }

        private void LoadLowStockDishes(object param = null)
        {
            int threshold =(int) SettingsHelper.Minimum_Stock_Alert; 
            var dishes = dishBL.GetLowStockDishes(threshold);
            LowStockDishes = new ObservableCollection<Dish>(dishes);
        }










    }
}

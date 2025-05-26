using Restaurant.Models.BusinessLogicLayer;
using Restaurant.Models.DataAccessLayer;
using Restaurant.Models.EntityLayer;
using Restaurant.ViewModels.Commands;
using Restaurant.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public class MenuViewModel : BasePropertyChanged
    {
        private readonly DishBL dishBL = new DishBL();
        private readonly MenuBL menuBL = new MenuBL();
        private readonly OrderBL orderBL = new OrderBL();
        private readonly CategoryBL categoryBL = new CategoryBL();

        private ObservableCollection<Dish> allDishes = new ObservableCollection<Dish>();
        public ObservableCollection<Dish> AllDishes
        {
            get => allDishes;
            set
            {
                allDishes = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Menu> allMenus = new ObservableCollection<Menu>();
        public ObservableCollection<Menu> AllMenus
        {
            get => allMenus;
            set
            {
                allMenus = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        public ObservableCollection<Category> Categories
        {
            get => categories;
            set
            {
                categories = value;
                NotifyPropertyChanged();
            }
        }


        private ObservableCollection<ShoppingCartItem<object>> cartItems;
        private Category selectedCategory;
        private string searchQuery;
        private string allergenQuery;
        private bool includeSearchTerm = false;
        private bool includeAllergen = false;
        private User currentUser;
        private bool isLoggedIn;
        private decimal totalCommandPrice;
        public decimal TotalCommandPrice
        {
            get => totalCommandPrice;
            set
            {
                totalCommandPrice = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<ShoppingCartItem<object>> CartItems
        {
            get => cartItems;
            set
            {
                cartItems = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsLoggedIn
        {
            get => isLoggedIn;
            set
            {
                isLoggedIn = value;
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

        private ICommand placeOrderCommand;
        public ICommand PlaceOrderCommand
        {
            get
            {
                if (placeOrderCommand == null)
                    placeOrderCommand = new RelayCommand(ExecutePlaceOrder);
                return placeOrderCommand;
            }
        }

        private void UpdateShoppingCartUI()
        {
            var items = ShoppingCart.Instance.GetItems();
            CartItems = new ObservableCollection<ShoppingCartItem<object>>(items);
            decimal subtotal = items.Sum(i => GetItemPrice(i.Item) * i.Quantity);

            // Aplică reducerea (dacă se îndeplinește condiția)
            decimal finalPrice = ApplyDiscounts(subtotal);

            // Adaugă livrarea dacă nu e gratuită
            finalPrice = AddDeliveryCost(finalPrice);

            TotalCommandPrice = finalPrice;

        }

        private decimal ApplyDiscounts(decimal basePrice)
        {
            if (basePrice >= SettingsHelper.Minimum_Order_For_Discount)
            {
                return basePrice * (1 - SettingsHelper.Discount_Percentage / 100);
            }

            if (IsLoggedIn && ((App)Application.Current).CurrentUser != null)
            {
                int userId = ((App)Application.Current).CurrentUser.UserID;
                OrderBL orderBL = new OrderBL();

                int ordersCount = orderBL.GetRecentOrders(userId, SettingsHelper.Days_For_Multiple_Orders);

                if (ordersCount >= SettingsHelper.Minimum_Orders_In_Days)
                {
                    return basePrice * (1 - SettingsHelper.Discount_Percentage / 100);
                }
            }

            return basePrice; 
        }

        private decimal AddDeliveryCost(decimal price)
        {
            if (price < SettingsHelper.Free_Delivery_Threshold)
            {
                return price + SettingsHelper.Delivery_Cost;
            }

            return price;
        }

        private decimal GetItemPrice(object item)
        {
            if (item is Dish dish) return dish.Price;
            if (item is Menu menu) return menu.CalculatedPrice;
            return 0;
        }

        private void ExecutePlaceOrder(object param)
        {
            var user = ((App)Application.Current).CurrentUser;

            var cartItems = ShoppingCart.Instance.GetItems();

            if (cartItems.Count == 0)
            {
                MessageBox.Show("Coșul este gol!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Creează comanda
            var order = new Order
            {
                UserID = user.UserID,
                OrderDate = DateTime.Now,
                Status = "Inregistrata"
            };

            var orderBL = new OrderBL();
            orderBL.PlaceOrder(order, cartItems);

            ShoppingCart.Instance.Clear();
            UpdateShoppingCartUI();

            MessageBox.Show("Comanda a fost plasată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private ICommand addToCartCommand;

        public ICommand AddToCartCommand
        {
            get
            {
                if (addToCartCommand == null)
                    addToCartCommand = new RelayCommand(ExecuteAddToCart);
                return addToCartCommand;
            }
        }
        private void ExecuteAddToCart(object obj)
        {
            if (obj is Dish dish)
            {
                if (!CanAddItem(dish))
                {
                    MessageBox.Show($"Nu mai avem suficientă cantitate pentru '{dish.Name}'.", "Stoc epuizat", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                ShoppingCart.Instance.AddItem(dish);
            }
            else if (obj is Menu menu)
            {
                if (!CanAddItem(menu))
                {
                    MessageBox.Show($"Nu mai avem suficientă cantitate pentru componentele din '{menu.Name}'.", "Stoc epuizat", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                ShoppingCart.Instance.AddItem(menu);
            }

            UpdateShoppingCartUI(); 
        }

        private ICommand removeFromCartCommand;
        public ICommand RemoveFromCartCommand
        {
            get
            {
                if (removeFromCartCommand == null)
                    removeFromCartCommand = new RelayCommand(ExecuteRemoveFromCart);
                return removeFromCartCommand;
            }
        }

        private void ExecuteRemoveFromCart(object obj)
        {
            if (obj is ShoppingCartItem<object> item)
            {
                ShoppingCart.Instance.RemoveItem(item.Item);
                UpdateShoppingCartUI();
            }
        }

        private ICommand openLoginCommand;
        public ICommand OpenLoginCommand
        {
            get
            {
                if (openLoginCommand == null)
                    openLoginCommand = new RelayCommand(OpenLogin);
                return openLoginCommand;
            }
        }

        private ICommand decreaseCommand;
        public ICommand DecreaseCommand
        {
            get
            {
                if (decreaseCommand == null)
                    decreaseCommand = new RelayCommand(ExecuteDecrease);
                return decreaseCommand;
            }
        }

        private void ExecuteDecrease(object obj)
        {
            if (obj is ShoppingCartItem<object> item)
            {
                var cartItem = ShoppingCart.Instance.GetItems()
                    .FirstOrDefault(i => Equals(i.Item, item.Item));

                if (cartItem != null)
                {
                    cartItem.Quantity -= 1;

                    if (cartItem.Quantity <= 0)
                    {
                        ShoppingCart.Instance.RemoveItem(item.Item);
                    }

                    UpdateShoppingCartUI();
                }
            }
        }

       
        private int GetTotalUsedGrams(Dish dish)
        {
            int totalUsed = 0;

            foreach (var cartItem in ShoppingCart.Instance.GetItems())
            {
                if (cartItem.Item is Dish d && d.DishID == dish.DishID)
                {
                    totalUsed +=(int) d.QuantityPerPortion * cartItem.Quantity;
                }
                else if (cartItem.Item is Menu m)
                {
                    foreach (var menuComponent in m.Items)
                    {
                        if (menuComponent.DishID == dish.DishID)
                        {
                            totalUsed += (int)(menuComponent.Quantity * cartItem.Quantity);
                        }
                    }
                }
            }

            return totalUsed;
        }

        public bool CanAddItem(object item)
        {
            var quantityInCart = ShoppingCart.Instance.GetItems()
                   .FirstOrDefault(i => i.Item == item)?.Quantity ?? 0;
            if (item is Dish dish)
            {
                int totalUsedGrams = GetTotalUsedGrams(dish);
                decimal requiredGramsNow = dish.QuantityPerPortion * (quantityInCart+1);
                return dishBL.GetTotalQuantity(dish.DishID) >= totalUsedGrams + requiredGramsNow;
            }
            else if (item is Menu menu)
            {
                foreach (var component in menu.Items)
                {
                    int totalUsedGrams = GetTotalUsedGrams(component.Dish);
                    decimal requiredGramsNow = component.Quantity * (quantityInCart+1);

                    if (dishBL.GetTotalQuantity(component.DishID) < totalUsedGrams + requiredGramsNow)
                        return false;
                }

                return true;
            }

            return false;
        }

        public void OpenLogin(object param)
        {
            var window = param as Window;

            var loginWindow = new LoginView();
            loginWindow.Show();

            window?.Close();
        }

        public MenuViewModel()
        {
            IsLoggedIn = false;
            LoadCategories();
            LoadDishesAndMenus();

            if (((App)Application.Current).CurrentUser != null && !((App)Application.Current).CurrentUser.IsEmployee)
            {
                LoadUserOrders();
            }
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


        private ObservableCollection<Order> allUserOrders = new ObservableCollection<Order>();
        public ObservableCollection<Order> AllUserOrders
        {
            get => allUserOrders;
            set
            {
                allUserOrders = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Order> activeUserOrders = new ObservableCollection<Order>();
        public ObservableCollection<Order> ActiveUserOrders
        {
            get => activeUserOrders;
            set
            {
                activeUserOrders = value;
                NotifyPropertyChanged();
            }
        }

        private void LoadUserOrders()
        {
            int userId = ((App)Application.Current).CurrentUser.UserID;

            var orders = orderBL.GetOrdersByUserId(userId);
            AllUserOrders = new ObservableCollection<Order>(orders);

            ActiveUserOrders = new ObservableCollection<Order>(
                orders.Where(o => o.Status == "Inregistrata" || o.Status == "Se pregateste" || o.Status == "a plecat la client")
            );
        }

        private ICommand cancelOrderCommand;
        public ICommand CancelOrderCommand
        {
            get
            {
                if (cancelOrderCommand == null)
                    cancelOrderCommand = new RelayCommand(ExecuteCancelOrder);
                return cancelOrderCommand;
            }
        }

        private void RestoreStockForOrder(int orderId)
        {
            var orderDetails = orderBL.GetOrderDetails(orderId);
            foreach (var detail in orderDetails)
            {
                if (detail.DishID.HasValue)
                {
                    orderBL.RestoreDishStock(detail.DishID.Value, detail.Quantity);
                }
            }
        }

        private void ExecuteCancelOrder(object obj)
        {
            if (obj is Order order && (order.Status == "Inregistrata" || order.Status == "Se pregateste" || order.Status == "a plecat la client"))
            {
                var result = MessageBox.Show("Sunteti sigur ca doriti sa anulati aceasta comanda?",
                                             "Confirmare",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    orderBL.CancelOrder(order.OrderID);
                    RestoreStockForOrder(order.OrderID);
                    LoadUserOrders(); 
                }
            }
        }
    }
}

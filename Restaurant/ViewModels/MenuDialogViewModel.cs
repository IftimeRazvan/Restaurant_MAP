using Restaurant.Models.EntityLayer;
using Restaurant.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Restaurant.ViewModels
{
    public class MenuDialogViewModel :BasePropertyChanged
    {
        public Menu Menu { get; }
        public ObservableCollection<Category> Categories { get; }
        public ObservableCollection<Dish> Dishes { get; }

        public ICommand AddItemCommand { get; set; }
        public ICommand RemoveItemCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        private Dish? _selectedDish;
        public Dish? SelectedDish
        {
            get => _selectedDish;
            set { _selectedDish = value; NotifyPropertyChanged(); }
        }

        public string QuantityInput { get; set; } = "1";

        public MenuDialogViewModel(Menu menu,ObservableCollection<Category> categories,ObservableCollection<Dish> dishes)
        {
            Menu = menu;
            Categories = categories;
            Dishes = dishes;

            AddItemCommand = new RelayCommand(_ => AddItem());
            RemoveItemCommand = new RelayCommand(mi => RemoveItem((MenuItem)mi));
            ConfirmCommand = new RelayCommand(w => Confirm((Window)w));
        }

        private void AddItem()
        {
            if (SelectedDish == null) return;
            if (!int.TryParse(QuantityInput, out int quantity) || quantity < 1) return;

            Menu.Items.Add(new MenuItem
            {
                Dish = SelectedDish,
                Quantity = quantity
            });
        }

        private void RemoveItem(MenuItem mi) => Menu.Items.Remove(mi);

        private void Confirm(Window dialog)
        {
            if (string.IsNullOrWhiteSpace(Menu.Name))
            {
                MessageBox.Show("Please enter a menu name.",
                                "Validation",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }
            if (Menu.Items.Count == 0)
            {
                MessageBox.Show("A menu must contain at least one dish.",
                                "Validation",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            dialog.DialogResult = true;
        }
    }

}


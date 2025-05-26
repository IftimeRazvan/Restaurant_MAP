using Restaurant.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Restaurant.Models.EntityLayer;
using Restaurant.ViewModels;

namespace Restaurant.Views
{
    /// <summary>
    /// Interaction logic for MenuDialogView.xaml
    /// </summary>
    public partial class MenuDialogView : Window
    {
        public MenuDialogView()
        {
            InitializeComponent();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(DishSelector.SelectedItem is Dish dish))
            {
                MessageBox.Show("Please select a dish to add.", "Validation",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(PortionInput.Text, out int size) || size < 1)
            {
                MessageBox.Show("Portion size must be a whole number ≥ 1.", "Validation",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var viewModel = (MenuDialogViewModel)DataContext!;
            var menu = viewModel.Menu;

            menu.Items.Add(new Models.EntityLayer.MenuItem
            {
                Dish = dish,
                Quantity = size
            });
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var mi = (Models.EntityLayer.MenuItem)((Button)sender).Tag!;
            var viewModel = (MenuDialogViewModel)DataContext!;
            viewModel.Menu.Items.Remove(mi);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (MenuDialogViewModel)DataContext!;
            var m = viewModel.Menu;

            if (string.IsNullOrWhiteSpace(m.Name))
            {
                MessageBox.Show("Please enter a menu name.", "Validation",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (m.Items.Count == 0)
            {
                MessageBox.Show("Please add at least one dish.", "Validation",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }

    }
}

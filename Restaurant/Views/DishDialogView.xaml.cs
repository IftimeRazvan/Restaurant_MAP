using Restaurant.Models.EntityLayer;
using Restaurant.ViewModels;
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

namespace Restaurant.Views
{
    /// <summary>
    /// Interaction logic for DishDialogView.xaml
    /// </summary>
    public partial class DishDialogView : Window
    {
        public DishDialogView()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var vm = (DishDialogViewModel)DataContext!;
            var d = vm.Dish;
            d.Allergens = vm.SelectedAllergens.Select(a => a.Name).ToList();
            if (string.IsNullOrWhiteSpace(d.Name))
            {
                MessageBox.Show("Please enter a name.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }

        private void AddAllergen_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as DishDialogViewModel;
            if (vm?.SelectedAllergens != null && !vm.SelectedAllergens.Any(a => a.AllergenID == vm.SelectedAllergen.AllergenID))
            {
                vm.SelectedAllergens.Add(vm.SelectedAllergen);
            }
        }

        private void RemoveAllergen_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Allergen allergen)
            {
                var vm = DataContext as DishDialogViewModel;
                vm?.SelectedAllergens.Remove(allergen);
            }
        }

    }
}

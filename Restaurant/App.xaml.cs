using Restaurant.Models.EntityLayer;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Restaurant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public User CurrentUser { get; set; }= null;
    }

}

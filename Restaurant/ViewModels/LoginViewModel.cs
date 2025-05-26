using Restaurant.Models.EntityLayer;
using Restaurant.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Restaurant.Models.DataAccessLayer;
using Restaurant.Models.BusinessLogicLayer;
using Restaurant.Views;

namespace Restaurant.ViewModels
{
    public class LoginViewModel : BasePropertyChanged
    {
        private string email;
        private string password;
        private string registerEmail;
        private string registerPassword;
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private string deliveryAddress;
        private string message;

        

        public string Email
        {
            get => email;
            set
            {
                email = value;
                NotifyPropertyChanged();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                NotifyPropertyChanged();
            }
        }

        public string RegisterEmail
        {
            get => registerEmail;
            set
            {
                registerEmail = value;
                NotifyPropertyChanged();
            }
        }

        public string RegisterPassword
        {
            get => registerPassword;
            set
            {
                registerPassword = value;
                NotifyPropertyChanged();
            }
        }

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                NotifyPropertyChanged();
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                NotifyPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                NotifyPropertyChanged();
            }
        }

        public string DeliveryAddress
        {
            get => deliveryAddress;
            set
            {
                deliveryAddress = value;
                NotifyPropertyChanged();
            }
        }

        public string Message
        {
            get => message;
            set
            {
                message = value;
                NotifyPropertyChanged();
            }
        }

        private readonly UserBL userBL;

        private ICommand registerCommand;
        public ICommand RegisterCommand
        {
            get
            {
                if (registerCommand == null)
                    registerCommand = new RelayCommand(_ => Message = userBL.RegisterUser(FirstName,LastName,RegisterEmail,PhoneNumber,DeliveryAddress,RegisterPassword));
                return registerCommand;
            }
        }

        private ICommand loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                    loginCommand = new RelayCommand(LoginUser);
                return loginCommand;
            }
        }

        public void LoginUser(object param)
        {
            var window = param as Window;

            User user = userBL.LoginUser(Email,Password);
            if (user != null)
            {
                if (user.IsEmployee)
                {
                    ((App)Application.Current).CurrentUser = user;
                    var EmployeeWindow= new EmployeeView();

                    window?.Close();

                    EmployeeWindow.Show();
                }
                else
                {
                    ((App)Application.Current).CurrentUser = user;
                    var menuWindow = new MenuView();
                    var menuViewModel = menuWindow.DataContext as MenuViewModel;
                    if (menuViewModel != null)
                    {
                        menuViewModel.IsLoggedIn = true;
                    }

                    window?.Close();

                    menuWindow.Show();
                }
            }

        }

        public LoginViewModel()
        {
            userBL = new UserBL();
        }
    }
}

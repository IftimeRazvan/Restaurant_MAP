using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.EntityLayer
{
    public class User : BasePropertyChanged
    {
        private int userID;
        private string firstName;
        private string lastName;
        private string email;
        private string phoneNumber;
        private string deliveryAddress;
        private string password;
        private bool isEmployee;

        public int UserID
        {
            get => userID;
            set
            {
                userID = value;
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

        public string Email
        {
            get => email;
            set
            {
                email = value;
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

        public string Password
        {
            get => password;
            set
            {
                password = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsEmployee
        {
            get => isEmployee;
            set
            {
                isEmployee = value;
                NotifyPropertyChanged();
            }
        }
    }
}

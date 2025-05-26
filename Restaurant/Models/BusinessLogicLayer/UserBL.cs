using Restaurant.Models.DataAccessLayer;
using Restaurant.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.BusinessLogicLayer
{
    public class UserBL
    {
        private readonly UserDAL userDAL = new UserDAL();

        public string RegisterUser(string firstName, string lastName, string email, string phoneNumber, string deliveryAddress, string password)
        {
           return  userDAL.RegisterUser(firstName,lastName,email,phoneNumber,deliveryAddress,password);
        }

        public User LoginUser(string email, string password)
        {
            return userDAL.LoginUser(email, password);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Restaurant.Models.DataAccessLayer;
using Restaurant.Models.EntityLayer;


namespace Restaurant.Models.BusinessLogicLayer
{
    public class AllergenBL
    {
        private readonly AllergenDAL allergenDAL = new AllergenDAL();

        

        public void InsertAllergen(string name)
        {
            allergenDAL.InsertAllergen(name);
        }

        public void DeleteAllergen(int allergenid)
        {
            allergenDAL.DeleteAllergen(allergenid);
        }

        public void UpdateAllergen(int allergenId, string newName)
        {
            allergenDAL.UpdateAllergen(allergenId,newName);
        }


        public ObservableCollection<Allergen> GetAllAllergens()
        {
            return allergenDAL.GetAllAllergens();
        }




    }
}

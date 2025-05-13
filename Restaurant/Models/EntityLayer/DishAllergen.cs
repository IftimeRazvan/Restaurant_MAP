using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.EntityLayer
{
    public class DishAllergen : BasePropertyChanged
    {
        private int dishID;
        private int allergenID;

        public int DishID
        {
            get => dishID;
            set
            {
                dishID = value;
                NotifyPropertyChanged();
            }
        }

        public int AllergenID
        {
            get => allergenID;
            set
            {
                allergenID = value;
                NotifyPropertyChanged();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.EntityLayer
{
    public class Allergen : BasePropertyChanged
    {
        private int allergenID;
        private string name;

        public int AllergenID
        {
            get => allergenID;
            set
            {
                allergenID = value;
                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }
    }
}

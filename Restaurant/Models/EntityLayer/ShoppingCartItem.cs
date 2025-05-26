using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.EntityLayer
{
    public class ShoppingCartItem<T>
    {
        public T Item { get; set; }
        public int Quantity { get; set; }
    }
}

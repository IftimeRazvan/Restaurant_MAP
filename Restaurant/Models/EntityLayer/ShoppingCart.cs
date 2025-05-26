using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.EntityLayer
{
    public class ShoppingCart
    {
        private static readonly ShoppingCart instance = new ShoppingCart();
        private List<ShoppingCartItem<object>> items = new List<ShoppingCartItem<object>>();

        private ShoppingCart() { }

        public static ShoppingCart Instance => instance;

        public void AddItem(object item, int quantity = 1)
        {
            var existing = items.FirstOrDefault(i => Equals(i.Item, item));
            if (existing != null)
                existing.Quantity += quantity;
            else
                items.Add(new ShoppingCartItem<object> { Item = item, Quantity = quantity });
        }

        public List<ShoppingCartItem<object>> GetItems() => items;

        public void Clear() => items.Clear();

        public void RemoveItem(object item)
        {
            var existing = items.FirstOrDefault(i => Equals(i.Item, item));
            if (existing != null)
            {
                items.Remove(existing);
            }
        }
    }
}

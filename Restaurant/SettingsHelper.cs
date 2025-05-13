using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Restaurant
{
    public static class SettingsHelper
    {
        // Setări privind stocul
        public static decimal Minimum_Stock_Alert => GetDecimalSetting("Minimum_Stock_Alert");

        // Setări privind meniurile
        public static decimal Discount_Menu_Percentage => GetDecimalSetting("Discount_Menu_Percentage");

        // Setări livrare
        public static decimal Free_Delivery_Threshold => GetDecimalSetting("Free_Delivery_Threshold");
        public static decimal Delivery_Cost => GetDecimalSetting("Delivery_Cost");

        // Setări reduceri pe comenzi
        public static decimal Minimum_Order_For_Discount => GetDecimalSetting("Minimum_Order_For_Discount");
        public static int Days_For_Multiple_Orders => GetIntSetting("Days_For_Multiple_Orders");
        public static int Minimum_Orders_In_Days => GetIntSetting("Minimum_Orders_In_Days");
        public static decimal Discount_Percentage => GetDecimalSetting("Discount_Percentage");


        // Metode ajutătoare pentru citirea valorilor
        private static decimal GetDecimalSetting(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (decimal.TryParse(value, out decimal result))
            {
                return result;
            }
            throw new InvalidOperationException($"Setarea '{key}' nu a fost găsită sau nu este un număr valid.");
        }

        private static int GetIntSetting(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            throw new InvalidOperationException($"Setarea '{key}' nu a fost găsită sau nu este un număr întreg valid.");
        }

        private static bool GetBoolSetting(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (bool.TryParse(value, out bool result))
            {
                return result;
            }
            // Valoare implicită sau excepție?
            return false;
        }
    }
}

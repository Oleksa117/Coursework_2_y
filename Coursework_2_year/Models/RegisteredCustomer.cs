using Coursework_2_year.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Models
{
    public class RegisteredCustomer : Customer, IDiscountable
    {
        public string Password { get; set; } = string.Empty;

        public decimal ApplyDiscount(decimal basePrice)
        {
            return basePrice * 0.9m;
        }

        public override string GetCustomerType() => "Registered";

        public override string GetInfo() =>
        $"{Name} (Зареєстрований) | {Email}";
    }
}

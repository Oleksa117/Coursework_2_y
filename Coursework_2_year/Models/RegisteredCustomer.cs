using Coursework_2_year.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Models
{
    public class RegisteredCustomer : Customer, IDiscountable
    {
        public decimal ApplyDiscount(decimal price)
        {
            return price * 0.9m;
        }
    }
}

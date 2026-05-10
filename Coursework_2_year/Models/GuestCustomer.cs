using System;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Xml.Linq;

namespace Coursework_2_year.Models
{
    public class GuestCustomer : Customer
    {
        public override string GetCustomerType() => "Guest";

        public override string GetInfo() => $"{Name} (Гість) | {ContactInfo}";
    }
}

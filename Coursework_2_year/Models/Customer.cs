using Coursework_2_year.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Models
{
    public abstract class Customer : IPrintable
    {
        public int Id { get; set; }

        public string LastName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ContactInfo { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public abstract string GetCustomerType();

        public abstract string GetInfo();

        public override string ToString() => $"{LastName} {FirstName}";
    }
}

using Coursework_2_year.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Models
{
    public abstract class Ticket : IBookable, IPrintable
    {
        public int Id { get; set; }
        public int ConcertId { get; set; }
        public string SeatLabel { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;

        public abstract decimal GetPrice();
        public abstract string GetTypeName();
        public abstract string GetInfo();

        public bool Book()
        {
            if (!IsAvailable)
                return false;

            IsAvailable = false;
            return true;
        }

        public void Cancel()
        {
            IsAvailable = true;
        }
    }
}

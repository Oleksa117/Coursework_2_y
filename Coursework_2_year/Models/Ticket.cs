using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public int ConcertId { get; set; }

        public string SeatLabel { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;

        public abstract decimal GetPrice();

        public abstract string GetTypeName();

        public abstract string GetInfo();
    }
}

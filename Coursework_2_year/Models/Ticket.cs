using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public int ConcertId { get; set; }

        public string Type { get; set; } = "";

        public double Price { get; set; }
    }
}

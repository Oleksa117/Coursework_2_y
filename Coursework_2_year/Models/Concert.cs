using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Models
{
    public class Concert
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Venue { get; set; } = "";

        public DateTime Date { get; set; }
    }
}

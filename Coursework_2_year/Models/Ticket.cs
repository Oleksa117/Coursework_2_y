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
        public string SeatLabel { get; set; } = string.Empty;// Наприклад, "A1", "B2" тощо
        public bool IsAvailable { get; set; } = true;

        public Customer? Owner { get; set; }// Власник квитка (може бути null, якщо квиток не проданий)

        public abstract decimal GetPrice();
        public abstract string GetTypeName();
        public abstract string GetInfo();

        public bool Book()// Метод для бронювання квитка
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

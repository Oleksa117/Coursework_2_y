using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Models
{
    public class Concert
    {
        public List<Ticket> Seats { get; set; } = new();// Ініціалізація списку квитків

        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Venue { get; set; } = "";

        public DateTime Date { get; set; }

        public Ticket? FindAvailableTicket(string type) =>
        Seats.FirstOrDefault(t => t.GetTypeName() == type && t.IsAvailable);// Пошук доступного квитка за типом

        public bool CheckAvailability(string type) =>
            Seats.Any(t => t.GetTypeName() == type && t.IsAvailable);// Перевірка наявності доступного квитка за типом

        public int GetAvailableCount(string type) =>
            Seats.Count(t => t.GetTypeName() == type && t.IsAvailable);// Підрахунок кількості доступних квитків за типом

        public int GetTotalCount(string type) =>
            Seats.Count(t => t.GetTypeName() == type);// Підрахунок загальної кількості квитків за типом
    }
}

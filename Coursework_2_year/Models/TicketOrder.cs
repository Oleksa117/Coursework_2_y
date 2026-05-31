using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Models
{
    public class TicketOrder
    {
        public int Id { get; set; }

        public Ticket Ticket { get; set; } = null!;

        public Customer Customer { get; set; } = null!;// Посилання на клієнта, який зробив замовлення

        public decimal FinalPrice { get; set; }

        public DateTime OrderTime { get; set; }

        public string GetReceiptText() =>
            $"Чек замовлення #{Id}\n" +
            $"Клієнт: {Customer.Name} ({Customer.GetCustomerType()})\n" +
            $"Квиток: {Ticket.GetTypeName()} | {Ticket.SeatLabel}\n" +
            $"Сума: {FinalPrice} грн\n" +
            $"Час: {OrderTime:dd.MM.yyyy HH:mm}";
    }
}

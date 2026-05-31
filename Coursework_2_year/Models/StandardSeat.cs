using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Models
{
    public class StandardSeat : Ticket
    {
        public override decimal GetPrice() => 800m;

        public override string GetTypeName() => "Standard";

        public override string GetInfo() =>$"Стандарт | {SeatLabel} | {(IsAvailable ? "Доступний" : "Продано")} | 800 грн";
    }
}

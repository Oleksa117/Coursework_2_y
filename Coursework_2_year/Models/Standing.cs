using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Models
{
    public class Standing : Ticket
    {
        public override decimal GetPrice() => 300m;

        public override string GetTypeName() => "Standing";

        public override string GetInfo() => $"Стояче | {SeatLabel} | {(IsAvailable ? "Доступний" : "Продано")} | 300 грн";
    }
}

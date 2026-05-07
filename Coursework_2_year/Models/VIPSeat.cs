using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Models
{
    public class VIPSeat : Ticket
    {
        public override decimal GetPrice() => 2000m;

        public override string GetTypeName() => "VIP";

        public override string GetInfo() =>
            $"VIP | {SeatLabel} | {(IsAvailable ? "Доступний" : "Продано")} | 2000 грн";
    }
}

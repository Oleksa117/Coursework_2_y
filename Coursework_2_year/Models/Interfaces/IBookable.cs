using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Models.Interfaces
{
    public interface IBookable
    {
        bool Book();
        void Cancel();
    }
}

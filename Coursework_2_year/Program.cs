using System;
using System.Windows.Forms;
using Coursework_2_year.Forms;

namespace Coursework_2_year
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
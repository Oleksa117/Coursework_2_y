using System;
using System.Windows.Forms;
using Coursework_2_year.Forms;
using Coursework_2_year.Data;

namespace Coursework_2_year
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            DatabaseHelper.CreateTables();

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
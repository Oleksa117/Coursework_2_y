using Microsoft.Data.Sqlite;

namespace Coursework_2_year.Data
{
    internal static class DatabaseHelper
    {
        private const string ConnectionString = "Data Source=tickets.db";

        public static void CreateTables()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = @"

                CREATE TABLE IF NOT EXISTS Users
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    Role TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Concerts
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Venue TEXT NOT NULL,
                    Date TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Tickets
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ConcertId INTEGER NOT NULL,
                    Type TEXT NOT NULL,
                    Price REAL NOT NULL,

                    FOREIGN KEY (ConcertId)
                        REFERENCES Concerts(Id)
                );

                CREATE TABLE IF NOT EXISTS Customers
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    Email TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Orders
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CustomerId INTEGER NOT NULL,
                    TicketId INTEGER NOT NULL,

                    FOREIGN KEY (CustomerId)
                        REFERENCES Customers(Id),

                    FOREIGN KEY (TicketId)
                        REFERENCES Tickets(Id)
                );
            ";

            command.ExecuteNonQuery();
        }
    }
}
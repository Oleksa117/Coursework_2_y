using Microsoft.Data.Sqlite;

namespace Coursework_2_year.Data
{
    public static class DatabaseHelper
    {
        private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "tickets.db");

        // Метод для отримання відкритого з'єднання з базою даних
        public static SqliteConnection GetConnection()
        {
            var conn = new SqliteConnection($"Data Source={DbPath}");// Створюємо з'єднання з базою даних
            conn.Open();
            return conn;
        }

        public static void CreateTables()
        {
            using var conn = GetConnection();// Отримуємо з'єднання з базою даних
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS Concerts (
                Id      INTEGER PRIMARY KEY AUTOINCREMENT,
                Title   TEXT    NOT NULL,
                Venue   TEXT    NOT NULL,
                Date    TEXT    NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Tickets (
                Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                ConcertId   INTEGER NOT NULL,
                Type        TEXT    NOT NULL,
                SeatLabel   TEXT    NOT NULL,
                IsAvailable INTEGER NOT NULL DEFAULT 1,
                FOREIGN KEY (ConcertId) REFERENCES Concerts(Id)
            );

            CREATE TABLE IF NOT EXISTS Customers (
                Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                Name        TEXT    NOT NULL,
                ContactInfo TEXT    NOT NULL,
                Type        TEXT    NOT NULL,
                Email       TEXT
            );

            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FirstName TEXT NOT NULL,
                LastName TEXT NOT NULL,
                Email TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL,
                Role TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS TicketOrders (
                Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                TicketId    INTEGER NOT NULL,
                CustomerId  INTEGER NOT NULL,
                FinalPrice  REAL    NOT NULL,
                OrderTime   TEXT    NOT NULL,
                FOREIGN KEY (TicketId)   REFERENCES Tickets(Id),
                FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
            );
            """;
            cmd.ExecuteNonQuery();// Виконуємо SQL-команди для створення таблиць, якщо вони ще не існують
        }
    }
}
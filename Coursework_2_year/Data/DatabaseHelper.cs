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

            var command = connection.CreateCommand();

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
            ";

            command.ExecuteNonQuery();
        }
    }
}
using Microsoft.Data.Sqlite;
using Coursework_2_year.Models;

namespace Coursework_2_year.Data
{
    public class TicketingSystem
    {
            private static TicketingSystem? _instance;

            public static TicketingSystem Instance
            {
                get
                {
                    if (_instance == null)
                        _instance = new TicketingSystem();

                    return _instance;
                }
            }

            private TicketingSystem()
            {
            }

            public User? GetUserByEmail(string email)
            {
                return null;
            }
       
        private const string ConnectionString = "Data Source=tickets.db";

        // Реєстрація нового користувача
        public static void Register(User user)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Users
                (FirstName, LastName, Email, Password, Role)
                VALUES
                (@firstName, @lastName, @email, @password, @role);
            ";

            command.Parameters.AddWithValue("@firstName", user.FirstName);
            command.Parameters.AddWithValue("@lastName", user.LastName);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@password", user.Password);
            command.Parameters.AddWithValue("@role", user.Role);

            command.ExecuteNonQuery();
        }

        // Авторизація
        public static User? Login(string email, string password)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT *
                FROM Users
                WHERE Email = @email
                AND Password = @password;
            ";

            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", password);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    Password = reader.GetString(4),
                    Role = reader.GetString(5)
                };
            }

            return null;
        }

        // Отримати всіх користувачів
        public static List<User> GetAllUsers()
        {
            List<User> users = new();

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Users";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    Password = reader.GetString(4),
                    Role = reader.GetString(5)
                });
            }

            return users;
        }

        internal TicketOrder PurchaseTicket(Concert concert,string ticketType,Customer customer)
        {
            return new TicketOrder();
        }
    }
}
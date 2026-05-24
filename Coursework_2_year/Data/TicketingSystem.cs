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

        private readonly List<Concert> _concerts = new();
        private readonly List<Customer> _customers = new();
        private readonly List<TicketOrder> _orders = new();

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

        internal TicketOrder PurchaseTicket(Concert concert, string ticketType, Customer customer)
        {
            return new TicketOrder();
        }

        public List<Concert> GetAllConcerts() => _concerts;

        public List<TicketOrder> GetAllOrders() => _orders;

        public decimal GetTotalRevenue() => _orders.Sum(o => o.FinalPrice);

        public Dictionary<string, decimal> GetRevenueByType() =>
        _orders
            .GroupBy(o => o.Ticket.GetTypeName())
            .ToDictionary(g => g.Key, g => g.Sum(o => o.FinalPrice));

        public void DeleteConcert(int concertId)
        {
            using var conn = DatabaseHelper.GetConnection();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            DELETE FROM Tickets
            WHERE ConcertId = @id;

            DELETE FROM Concerts
            WHERE Id = @id;";

            cmd.Parameters.AddWithValue("@id", concertId);

            cmd.ExecuteNonQuery();

            LoadAll();
        }// якщо у тебе є внутрішній список концертів

        public void AddConcert(string title, string venue, DateTime date)
        {
            using var conn = DatabaseHelper.GetConnection();

            long concertId = InsertConcert(conn,title,venue,date.ToString("yyyy-MM-dd"));

            for (int i = 1; i <= 5; i++)
                InsertTicket(conn, concertId, "VIP", $"A{i:D2}");

            for (int i = 1; i <= 10; i++)
                InsertTicket(conn, concertId, "Standard", $"B{i:D2}");

            for (int i = 1; i <= 20; i++)
                InsertTicket(conn, concertId, "Standing", "Standing");

            LoadAll();
        }

        public void LoadAll()
        {
            _concerts.Clear();
            _customers.Clear();
            _orders.Clear();

            using var conn = DatabaseHelper.GetConnection();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Name, ContactInfo, Type, Email FROM Customers;";
                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    Customer c = r.GetString(3) == "Registered"
                        ? new RegisteredCustomer
                        {
                            Email = r.IsDBNull(4) ? "" : r.GetString(4),

                        }
                        : new GuestCustomer();

                    c.Id = r.GetInt32(0);
                    c.Name = r.GetString(1);
                    c.ContactInfo = r.GetString(2);
                    _customers.Add(c);
                }
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Title, Venue, Date FROM Concerts;";
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    _concerts.Add(new Concert
                    {
                        Id = r.GetInt32(0),
                        Title = r.GetString(1),
                        Venue = r.GetString(2),
                        Date = DateTime.Parse(r.GetString(3)),
                    });
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, ConcertId, Type, SeatLabel, IsAvailable FROM Tickets;";
                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    Ticket t = r.GetString(2) switch
                    {
                        "VIP" => new VIPSeat(),
                        "Standard" => new StandardSeat(),
                        _ => new Standing(),
                    };
                    t.Id = r.GetInt32(0);
                    t.ConcertId = r.GetInt32(1);
                    t.SeatLabel = r.GetString(3);
                    t.IsAvailable = r.GetInt32(4) == 1;

                    _concerts.FirstOrDefault(c => c.Id == t.ConcertId)?.Seats.Add(t);
                }
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, TicketId, CustomerId, FinalPrice, OrderTime FROM TicketOrders;";
                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    int ticketId = r.GetInt32(1);
                    int customerId = r.GetInt32(2);

                    var ticket = _concerts.SelectMany(c => c.Seats).FirstOrDefault(t => t.Id == ticketId);
                    var customer = _customers.FirstOrDefault(c => c.Id == customerId);
                    if (ticket == null || customer == null) continue;

                    _orders.Add(new TicketOrder
                    {
                        Id = r.GetInt32(0),
                        Ticket = ticket,
                        Customer = customer,
                        FinalPrice = (decimal)r.GetDouble(3),
                        OrderTime = DateTime.Parse(r.GetString(4)),
                    });
                }
            }

        }

        private static long InsertConcert(SqliteConnection conn, string title, string venue, string date)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Concerts (Title, Venue, Date) VALUES ($t,$v,$d); SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$t", title);
            cmd.Parameters.AddWithValue("$v", venue);
            cmd.Parameters.AddWithValue("$d", date);
            return (long)cmd.ExecuteScalar()!;
        }

        private static void InsertTicket(SqliteConnection conn, long concertId, string type, string label)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Tickets (ConcertId, Type, SeatLabel, IsAvailable) VALUES ($c,$t,$l,1);";
            cmd.Parameters.AddWithValue("$c", concertId);
            cmd.Parameters.AddWithValue("$t", type);
            cmd.Parameters.AddWithValue("$l", label);
            cmd.ExecuteNonQuery();
        }

        public void UpdateConcert(int concertId, string title, string venue, DateTime date)
        {
            using var conn = DatabaseHelper.GetConnection();

            using var cmd = conn.CreateCommand();

            cmd.CommandText =
            """
            UPDATE Concerts
            SET Title = $title,
            Venue = $venue,
            Date = $date
            WHERE Id = $id;
            """;

            cmd.Parameters.AddWithValue("$title", title);
            cmd.Parameters.AddWithValue("$venue", venue);
            cmd.Parameters.AddWithValue("$date", date.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("$id", concertId);

            cmd.ExecuteNonQuery();

            var concert = _concerts.First(c => c.Id == concertId);

            concert.Title = title;
            concert.Venue = venue;
            concert.Date = date;
        }
    }
}


   

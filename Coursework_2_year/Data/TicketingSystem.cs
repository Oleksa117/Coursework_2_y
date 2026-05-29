using Coursework_2_year.Models;
using Coursework_2_year.Models.Interfaces;
using Coursework_2_year.Data;
using Microsoft.Data.Sqlite;

namespace Coursework_2_year.Data
{
    public class TicketingSystem
    {
        private static TicketingSystem? _instance;

        public static TicketingSystem Instance => _instance ??= new TicketingSystem();

        private readonly List<Concert> _concerts = new();
        private readonly List<Customer> _customers = new();
        private readonly List<TicketOrder> _orders = new();
        bool isRegisteredUser = true;
        private TicketingSystem()
        {
            InitializeDatabase();
            SeedIfEmpty();
            LoadAll();
        }

        public void InitializeDatabase() => DatabaseHelper.CreateTables();

        public void SeedIfEmpty()
        {
            using var conn = DatabaseHelper.GetConnection();
            using var check = conn.CreateCommand();
            check.CommandText = "SELECT COUNT(*) FROM Concerts;";
            if ((long)(check.ExecuteScalar() ?? 0L) > 0) return;

            var concerts = new[]
            {
            ("Rock Fest 2025",    "Київ",     "2026-06-15"),
            ("Jazz Evening",      "Львів",    "2026-07-20"),
            ("Electronic Night",  "Харків",   "2026-08-10"),
            ("Pop Stars Gala",    "Одеса",    "2026-09-05"),
            ("Metal Madness",     "Дніпро",   "2026-10-18"),
            ("Classical Night",   "Київ",     "2026-11-22"),
            ("Folk Festival",     "Полтава",  "2026-12-07"),
            ("New Year Concert",  "Київ",     "2027-01-01"),
        };

            var concertIds = new List<long>();
            foreach (var (title, venue, date) in concerts)
                concertIds.Add(InsertConcert(conn, title, venue, date));

            foreach (var cid in concertIds)
            {
                for (int i = 1; i <= 5; i++) InsertTicket(conn, cid, "VIP", $"A{i:D2}");
                for (int i = 1; i <= 10; i++) InsertTicket(conn, cid, "Standard", $"B{i:D2}");
                for (int i = 1; i <= 20; i++) InsertTicket(conn, cid, "Standing", "Standing");
            }

            // ── 20 зареєстрованих клієнтів ───────────────────────────────────────
            long olena = InsertCustomer(conn, "Олена Коваль", "+380501111001", "Registered", "olena@mail.com");
            long ivan = InsertCustomer(conn, "Іван Мельник", "+380671111002", "Registered", "ivan@mail.com");
            long maria = InsertCustomer(conn, "Марія Шевченко", "+380931111003", "Registered", "maria@mail.com");
            long oleksiy = InsertCustomer(conn, "Олексій Бондаренко", "+380681111004", "Registered", "oleksiy@mail.com");
            long dmytro = InsertCustomer(conn, "Дмитро Лисенко", "+380991111005", "Registered", "dmytro@mail.com");
            long tetyana = InsertCustomer(conn, "Тетяна Павленко", "+380631111006", "Registered", "tetyana@mail.com");
            long andriy = InsertCustomer(conn, "Андрій Гриценко", "+380731111007", "Registered", "andriy@mail.com");
            long natalia = InsertCustomer(conn, "Наталія Романенко", "+380661111008", "Registered", "natalia@mail.com");
            long serhiy = InsertCustomer(conn, "Сергій Зінченко", "+380501111009", "Registered", "serhiy@mail.com");
            long yuliia = InsertCustomer(conn, "Юлія Власенко", "+380671111010", "Registered", "yuliia@mail.com");
            long maksym = InsertCustomer(conn, "Максим Петренко", "+380931111011", "Registered", "maksym@mail.com");
            long viktoria = InsertCustomer(conn, "Вікторія Ткаченко", "+380681111012", "Registered", "viktoria@mail.com");
            long bohdan = InsertCustomer(conn, "Богдан Олійник", "+380991111013", "Registered", "bohdan@mail.com");
            long iryna = InsertCustomer(conn, "Ірина Семенченко", "+380631111014", "Registered", "iryna@mail.com");
            long vasyl = InsertCustomer(conn, "Василь Ковальчук", "+380731111015", "Registered", "vasyl@mail.com");
            long oksana = InsertCustomer(conn, "Оксана Мартиненко", "+380661111016", "Registered", "oksana@mail.com");
            long ruslan = InsertCustomer(conn, "Руслан Яковенко", "+380501111017", "Registered", "ruslan@mail.com");
            long alina = InsertCustomer(conn, "Аліна Сидоренко", "+380671111018", "Registered", "alina@mail.com");
            long yevhen = InsertCustomer(conn, "Євген Нечипоренко", "+380931111019", "Registered", "yevhen@mail.com");
            long kateryna = InsertCustomer(conn, "Катерина Борисенко", "+380681111020", "Registered", "kateryna@mail.com");

            // ── 20 гостей ─────────────────────────────────────────────────────────
            long anonGuest = InsertCustomer(conn, "Гість", "—", "Guest", null);
            long petro = InsertCustomer(conn, "Петро Савченко", "+380731111021", "Guest", null);
            long nadiia = InsertCustomer(conn, "Надія Кравченко", "+380661111022", "Guest", null);
            long mykola = InsertCustomer(conn, "Микола Кузьменко", "+380501111023", "Guest", null);
            long hanna = InsertCustomer(conn, "Ганна Хоменко", "+380671111024", "Guest", null);
            long leonid = InsertCustomer(conn, "Леонід Супруненко", "+380931111025", "Guest", null);
            long tamara = InsertCustomer(conn, "Тамара Білоус", "+380681111026", "Guest", null);
            long oleh = InsertCustomer(conn, "Олег Луценко", "+380991111027", "Guest", null);
            long svitlana = InsertCustomer(conn, "Світлана Панченко", "+380631111028", "Guest", null);
            long fedir = InsertCustomer(conn, "Федір Кириленко", "+380731111029", "Guest", null);
            long liudmyla = InsertCustomer(conn, "Людмила Степаненко", "+380661111030", "Guest", null);
            long anton = InsertCustomer(conn, "Антон Захаренко", "+380501111031", "Guest", null);
            long daria = InsertCustomer(conn, "Дар'я Мусієнко", "+380671111032", "Guest", null);
            long vladyslav = InsertCustomer(conn, "Владислав Гнатенко", "+380931111033", "Guest", null);
            long zoia = InsertCustomer(conn, "Зоя Заєць", "+380681111034", "Guest", null);
            long pylyp = InsertCustomer(conn, "Пилип Бабич", "+380991111035", "Guest", null);
            long uliana = InsertCustomer(conn, "Уляна Литвиненко", "+380631111036", "Guest", null);
            long herman = InsertCustomer(conn, "Герман Вернигора", "+380731111037", "Guest", null);
            long khrystyna = InsertCustomer(conn, "Христина Даниленко", "+380661111038", "Guest", null);
            long larysa = InsertCustomer(conn, "Лариса Полтавець", "+380501111039", "Guest", null);

            void Buy(long concertId, string type, string label, long customerId, decimal price, int daysAgo)
            {
                long tid = GetTicketId(conn, concertId, type, label);
                InsertOrder(conn, tid, customerId, price, DateTime.Now.AddDays(-daysAgo));
                MarkTicketSold(conn, tid);
            }

            // Rock Fest (0) — 5 продажів, дні 40–36
            Buy(concertIds[0], "VIP", "A01", olena, 1800m, 40);
            Buy(concertIds[0], "VIP", "A02", ivan, 1800m, 39);
            Buy(concertIds[0], "Standard", "B01", maria, 720m, 38);
            Buy(concertIds[0], "Standard", "B02", petro, 800m, 37);
            Buy(concertIds[0], "Standing", "Standing", hanna, 300m, 36);

            // Jazz Evening (1) — 5 продажів, дні 35–31
            Buy(concertIds[1], "VIP", "A01", oleksiy, 1800m, 35);
            Buy(concertIds[1], "Standard", "B01", dmytro, 720m, 34);
            Buy(concertIds[1], "Standard", "B02", tetyana, 720m, 33);
            Buy(concertIds[1], "Standing", "Standing", nadiia, 300m, 32);
            Buy(concertIds[1], "Standing", "Standing", mykola, 300m, 31);

            // Electronic Night (2) — 5 продажів, дні 30–26
            Buy(concertIds[2], "VIP", "A01", andriy, 1800m, 30);
            Buy(concertIds[2], "Standard", "B01", natalia, 720m, 29);
            Buy(concertIds[2], "Standard", "B02", serhiy, 720m, 28);
            Buy(concertIds[2], "Standard", "B03", leonid, 800m, 27);
            Buy(concertIds[2], "Standing", "Standing", tamara, 300m, 26);

            // Pop Stars Gala (3) — 5 продажів, дні 25–21
            Buy(concertIds[3], "VIP", "A01", yuliia, 1800m, 25);
            Buy(concertIds[3], "VIP", "A02", maksym, 1800m, 24);
            Buy(concertIds[3], "Standard", "B01", viktoria, 720m, 23);
            Buy(concertIds[3], "Standard", "B02", oleh, 800m, 22);
            Buy(concertIds[3], "Standing", "Standing", svitlana, 300m, 21);

            // Metal Madness (4) — 5 продажів, дні 20–16
            Buy(concertIds[4], "VIP", "A01", bohdan, 1800m, 20);
            Buy(concertIds[4], "Standard", "B01", iryna, 720m, 19);
            Buy(concertIds[4], "Standard", "B02", vasyl, 720m, 18);
            Buy(concertIds[4], "Standing", "Standing", fedir, 300m, 17);
            Buy(concertIds[4], "Standing", "Standing", liudmyla, 300m, 16);

            // Classical Night (5) — 5 продажів, дні 15–11
            Buy(concertIds[5], "VIP", "A01", oksana, 1800m, 15);
            Buy(concertIds[5], "VIP", "A02", daria, 2000m, 14);
            Buy(concertIds[5], "Standard", "B01", ruslan, 720m, 13);
            Buy(concertIds[5], "Standard", "B02", alina, 720m, 12);
            Buy(concertIds[5], "Standing", "Standing", anton, 300m, 11);

            // Folk Festival (6) — 5 продажів, дні 10–6
            Buy(concertIds[6], "VIP", "A01", yevhen, 1800m, 10);
            Buy(concertIds[6], "Standard", "B01", kateryna, 720m, 9);
            Buy(concertIds[6], "Standard", "B02", vladyslav, 800m, 8);
            Buy(concertIds[6], "Standing", "Standing", zoia, 300m, 7);
            Buy(concertIds[6], "Standing", "Standing", pylyp, 300m, 6);

            // New Year Concert (7) — 5 продажів, дні 5–1
            Buy(concertIds[7], "VIP", "A01", uliana, 2000m, 5);
            Buy(concertIds[7], "Standard", "B01", herman, 800m, 4);
            Buy(concertIds[7], "Standard", "B02", khrystyna, 800m, 3);
            Buy(concertIds[7], "Standing", "Standing", larysa, 300m, 2);
            Buy(concertIds[7], "Standing", "Standing", anonGuest, 300m, 1);
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
                using var r = cmd.ExecuteReader();//
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

        // ── Public API ────────────────────────────────────────────────────────────

        public List<Concert> GetAllConcerts() => _concerts;

        public Concert? GetConcertById(int id) => _concerts.FirstOrDefault(c => c.Id == id);

        public List<Customer> GetAllCustomers() => _customers;

        public List<TicketOrder> GetAllOrders() => _orders;

        public TicketOrder? PurchaseTicket(Concert concert, string ticketType, Customer customer)
        {
            var ticket = concert.FindAvailableTicket(ticketType);
            if (ticket == null) return null;

            decimal price = ticket.GetPrice();

            if (customer is RegisteredCustomer)
                price *= 0.9m;

            if (!ticket.Book()) return null;

            var now = DateTime.Now;

            using var conn = DatabaseHelper.GetConnection();

            if (customer.Id == 0)
            {
                customer.Id = (int)InsertCustomer(conn,
                    customer.Name,
                    customer.ContactInfo,
                    customer.GetCustomerType(),
                    (customer as RegisteredCustomer)?.Email);
                _customers.Add(customer);
            }

            MarkTicketSold(conn, ticket.Id);
            long orderId = InsertOrder(conn, ticket.Id, customer.Id, price, now);

            var order = new TicketOrder
            {
                Id = (int)orderId,
                Ticket = ticket,
                Customer = customer,
                FinalPrice = price,
                OrderTime = now,
            };
            ticket.Owner = customer;
            _orders.Add(order);
            return order;
        }

        public void CancelOrder(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return;

            order.Ticket.Cancel();

            using var conn = DatabaseHelper.GetConnection();
            MarkTicketAvailable(conn, order.Ticket.Id);

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM TicketOrders WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", orderId);
            cmd.ExecuteNonQuery();

            _orders.Remove(order);
        }

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
        }

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

        private static long InsertCustomer(
        SqliteConnection conn,
        string name,
        string contact,
        string type,
        string? email)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            INSERT INTO Customers
            (Name, ContactInfo, Type, Email)
            VALUES ($n, $c, $t, $e);

            SELECT last_insert_rowid();
            ";
            cmd.Parameters.AddWithValue("$n", name);
            cmd.Parameters.AddWithValue("$c", contact);
            cmd.Parameters.AddWithValue("$t", type);
            cmd.Parameters.AddWithValue("$e", (object?)email ?? DBNull.Value);
            return (long)cmd.ExecuteScalar()!;
        }

        private static long InsertOrder(SqliteConnection conn, long ticketId, long customerId,
                                         decimal price, DateTime time)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
            INSERT INTO TicketOrders (TicketId, CustomerId, FinalPrice, OrderTime)
            VALUES ($ti,$cu,$p,$ot);
            SELECT last_insert_rowid();
            """;
            cmd.Parameters.AddWithValue("$ti", ticketId);
            cmd.Parameters.AddWithValue("$cu", customerId);
            cmd.Parameters.AddWithValue("$p", (double)price);
            cmd.Parameters.AddWithValue("$ot", time.ToString("o"));
            return (long)cmd.ExecuteScalar()!;
        }

        private static long GetTicketId(SqliteConnection conn, long concertId, string type, string label)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id FROM Tickets WHERE ConcertId=$c AND Type=$t AND SeatLabel=$l AND IsAvailable=1 LIMIT 1;";
            cmd.Parameters.AddWithValue("$c", concertId);
            cmd.Parameters.AddWithValue("$t", type);
            cmd.Parameters.AddWithValue("$l", label);
            return (long)cmd.ExecuteScalar()!;
        }

        private static void MarkTicketSold(SqliteConnection conn, long ticketId)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Tickets SET IsAvailable=0 WHERE Id=$id;";
            cmd.Parameters.AddWithValue("$id", ticketId);
            cmd.ExecuteNonQuery();
        }

        private static void MarkTicketAvailable(SqliteConnection conn, long ticketId)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Tickets SET IsAvailable=1 WHERE Id=$id;";
            cmd.Parameters.AddWithValue("$id", ticketId);
            cmd.ExecuteNonQuery();
        }

        public void RegisterUser(string firstName, string lastName, string email, string password)
        {
            using var conn = DatabaseHelper.GetConnection();

            using (var check = conn.CreateCommand())
            {
                check.CommandText =
                    "SELECT COUNT(*) FROM Users WHERE Email=@em;";

                check.Parameters.AddWithValue("@em", email);

                if ((long)check.ExecuteScalar()! > 0)
                    throw new Exception("Користувач з такою поштою вже існує.");
            }

            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
            INSERT INTO Users
            (FirstName, LastName, Email, Password, Role)
            VALUES
            (@fn, @ln, @em, @pw, 'Client');
            ";

            cmd.Parameters.AddWithValue("@fn", firstName);
            cmd.Parameters.AddWithValue("@ln", lastName);
            cmd.Parameters.AddWithValue("@em", email);
            cmd.Parameters.AddWithValue("@pw", password);

            cmd.ExecuteNonQuery();
        }

        public string? Login(string email, string password)
        {
            using var conn = DatabaseHelper.GetConnection();

            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
            SELECT Role
            FROM Users
            WHERE Email = @email
            AND Password = @password;
            ";

            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);

            return cmd.ExecuteScalar()?.ToString();
        }

        public (string FirstName, string LastName)? GetUserByEmail(string email)
        {
            using var conn = DatabaseHelper.GetConnection();

            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
            SELECT FirstName, LastName
            FROM Users
            WHERE Email = @email;
            ";

            cmd.Parameters.AddWithValue("@email", email);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return (
                reader.GetString(0),
                reader.GetString(1)
            );
        }
    }
}

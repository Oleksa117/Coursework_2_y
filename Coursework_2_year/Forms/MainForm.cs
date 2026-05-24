using Coursework_2_year.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Forms
{
    public class MainForm : Form
    {
        private DataGridView dgvConcerts = null!;
        private Button btnView = null!;
        private Button btnReport = null!;
        private Button btnLogin = null!;
        private Button btnAddConcert = null!;
        private Button btnDeleteConcert = null!;
        private Button btnEditConcert = null!;

        private string _role = null!;
        private string _currentUserEmail = "";
        private bool _isLoggedIn = false;

        public MainForm()
        {
            InitializeComponent();

            _role = "Anonymous";
            _currentUserEmail = "";

            LoadData();
            ConfigurePermissions();
        }

        private void InitializeComponent()
        {
            Text = "Система бронювання квитків";
            ClientSize = new Size(940, 520);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10f);
            BackColor = SystemColors.Control;

            dgvConcerts = new DataGridView
            {
                Location = new Point(12, 12),
                Size = new Size(920, 420),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.LightGray,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                },
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                RowHeadersVisible = false,
                BorderStyle = BorderStyle.Fixed3D,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            dgvConcerts.Columns.AddRange(
                new DataGridViewTextBoxColumn { Name = "colTitle", HeaderText = "Назва", FillWeight = 32 },
                new DataGridViewTextBoxColumn { Name = "colVenue", HeaderText = "Місто", FillWeight = 18 },
                new DataGridViewTextBoxColumn { Name = "colDate", HeaderText = "Дата", FillWeight = 14 },
                new DataGridViewTextBoxColumn { Name = "colVIP", HeaderText = "VIP", FillWeight = 12 },
                new DataGridViewTextBoxColumn { Name = "colStandard", HeaderText = "Стандарт", FillWeight = 12 },
                new DataGridViewTextBoxColumn { Name = "colStanding", HeaderText = "Стояче", FillWeight = 12 }
            );

            // Кнопка Вхід — зліва
            btnLogin = new Button
            {
                Text = "Вхід",
                Location = new Point(12, 444),
                Size = new Size(120, 34)
            };
            btnLogin.Click += BtnLogin_Click;

            // Кнопки адміністратора — посередині
            btnReport = new Button
            {
                Text = "Звіт доходів",
                Location = new Point(144, 444),
                Size = new Size(140, 34),
            };
            btnReport.Click += BtnReport_Click;

            btnAddConcert = new Button
            {
                Text = "Додати концерт",
                Location = new Point(292, 444),
                Size = new Size(140, 34)
            };
            btnAddConcert.Click += BtnAddConcert_Click;

            btnDeleteConcert = new Button
            {
                Text = "Видалити концерт",
                Location = new Point(440, 444),
                Size = new Size(140, 34)
            };
            btnDeleteConcert.Click += BtnDeleteConcert_Click;

            btnEditConcert = new Button
            {
                Text = "Редагувати",
                Location = new Point(588, 444),
                Size = new Size(140, 34)
            };
            btnEditConcert.Click += BtnEditConcert_Click;

            // Кнопка Переглянути концерт — справа
            btnView = new Button
            {
                Text = "Переглянути концерт",
                Location = new Point(740, 444),
                Size = new Size(190, 34),
            };
            btnView.Click += BtnView_Click;

            Controls.AddRange(new Control[]
            {
                dgvConcerts,
                btnLogin,
                btnReport,
                btnAddConcert,
                btnDeleteConcert,
                btnEditConcert,
                btnView
            });
        }

        private void LoadData()
        {
            dgvConcerts.Rows.Clear();
            foreach (var c in TicketingSystem.Instance.GetAllConcerts())
            {
                dgvConcerts.Rows.Add(
                    c.Title,
                    c.Venue,
                    c.Date.ToString("dd.MM.yyyy"),
                    $"{c.GetAvailableCount("VIP")}/{c.GetTotalCount("VIP")}",
                    $"{c.GetAvailableCount("Standard")}/{c.GetTotalCount("Standard")}",
                    $"{c.GetAvailableCount("Standing")}/{c.GetTotalCount("Standing")}"
                );
            }
        }

        private void BtnView_Click(object? sender, EventArgs e)
        {
            if (!_isLoggedIn)
            {
                MessageBox.Show("Спочатку увійдіть у систему.");
                return;
            }

            if (dgvConcerts.CurrentRow == null)
                return;

            int idx = dgvConcerts.CurrentRow.Index;

            var concerts = TicketingSystem.Instance.GetAllConcerts();

            if (idx < 0 || idx >= concerts.Count)
                return;

            using var form = new ConcertForm(concerts[idx], _currentUserEmail);
            form.ShowDialog(this);

            LoadData();
        }

        private void BtnReport_Click(object? sender, EventArgs e)
        {
            using var form = new ReportForm();
            form.ShowDialog(this);
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            using var login = new LoginForm();

            if (login.ShowDialog() == DialogResult.OK)
            {
                _role = login.SelectedRole;
                _currentUserEmail = login.UserEmail;
                _isLoggedIn = true;

                ConfigurePermissions();
            }
        }

        private void BtnDeleteConcert_Click(object? sender, EventArgs e)
        {
            if (_role != "Admin")
            {
                MessageBox.Show("Лише адміністратор може видаляти концерти!");
                return;
            }

            if (dgvConcerts.CurrentRow == null)
                return;

            int index = dgvConcerts.CurrentRow.Index;

            var concerts = TicketingSystem.Instance.GetAllConcerts();

            if (index < 0 || index >= concerts.Count)
                return;

            var concert = concerts[index];

            TicketingSystem.Instance.DeleteConcert(concert.Id);

            LoadData();
        }

        private void BtnAddConcert_Click(object? sender, EventArgs e)
        {
            if (_role != "Admin")
            {
                MessageBox.Show("Лише адміністратор може додавати концерти!");
                return;
            }

            TicketingSystem.Instance.AddConcert(
                "Новий концерт",
                "Черкаси",
                DateTime.Today.AddMonths(1));

            LoadData();
        }

        private void BtnEditConcert_Click(object? sender, EventArgs e)
        {
            if (dgvConcerts.CurrentRow == null)
                return;

            int index = dgvConcerts.CurrentRow.Index;

            var concert =TicketingSystem.Instance.GetAllConcerts()[index];

            string title =Microsoft.VisualBasic.Interaction.InputBox(
                    "Назва концерту:",
                    "Редагування",
                    concert.Title);

            string venue =Microsoft.VisualBasic.Interaction.InputBox(
                    "Місто:",
                    "Редагування",
                    concert.Venue);

            string dateText =Microsoft.VisualBasic.Interaction.InputBox(
                    "Дата (yyyy-MM-dd):",
                    "Редагування",
                    concert.Date.ToString("yyyy-MM-dd"));

            if (!DateTime.TryParse(dateText, out DateTime newDate))
            {
                MessageBox.Show("Невірна дата");
                return;
            }

            TicketingSystem.Instance.UpdateConcert(concert.Id,title,venue,newDate);
            LoadData();
        }

        private void ConfigurePermissions()
        {
            btnReport.Visible = false;
            btnAddConcert.Visible = false;
            btnDeleteConcert.Visible = false;
            btnEditConcert.Visible = false;

            btnView.Visible = _isLoggedIn;

            if (_role == "Admin")
            {
                btnReport.Visible = true;
                btnAddConcert.Visible = true;
                btnDeleteConcert.Visible = true;
                btnEditConcert.Visible = true;

                btnView.Visible = false;
            }
        }
    }
}

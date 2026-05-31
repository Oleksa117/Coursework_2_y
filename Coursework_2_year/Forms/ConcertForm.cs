using Coursework_2_year.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Forms
{
    public class ConcertForm : Form
    {
        private readonly Concert _concert;
        private Label lblTitle = null!;
        private Label lblVenue = null!;
        private Label lblDate = null!;
        private DataGridView dgvTickets = null!;
        private Button btnBuy = null!;
        private readonly string _currentUserEmail;

        public ConcertForm(Concert concert, string currentUserEmail)
        {
            _concert = concert;
            _currentUserEmail = currentUserEmail;

            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            Text = "Концерт";
            ClientSize = new Size(706, 520);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10f);
            BackColor = SystemColors.Control;
            
            lblTitle = new Label
            {
                Location = new Point(12, 12),
                Size = new Size(682, 26),
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            };
            lblVenue = new Label { Location = new Point(12, 44), Size = new Size(682, 22) };
            lblDate = new Label { Location = new Point(12, 70), Size = new Size(682, 22) };

            dgvTickets = new DataGridView
            {
                Location = new Point(12, 100),
                Size = new Size(682, 356),
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
            };
            dgvTickets.Columns.AddRange(
                new DataGridViewTextBoxColumn { Name = "colType", HeaderText = "Тип", FillWeight = 25 },
                new DataGridViewTextBoxColumn { Name = "colLabel", HeaderText = "Місце", FillWeight = 25 },
                new DataGridViewTextBoxColumn { Name = "colAvail", HeaderText = "Наявність", FillWeight = 25 },
                new DataGridViewTextBoxColumn { Name = "colPrice", HeaderText = "Ціна", FillWeight = 25 }
            );

            btnBuy = new Button
            {
                Text = "Купити квиток",
                Location = new Point(462, 470),
                Size = new Size(232, 36),
            };
            btnBuy.Click += BtnBuy_Click;

            Controls.AddRange(new Control[] { lblTitle, lblVenue, lblDate, dgvTickets, btnBuy });// Додано кнопку до форми
        }

        private void LoadData()
        {
            lblTitle.Text = _concert.Title;
            lblVenue.Text = $"Місце: {_concert.Venue}";
            lblDate.Text = $"Дата: {_concert.Date:dd.MM.yyyy}";

            dgvTickets.Rows.Clear();
            foreach (var t in _concert.Seats)// Додано цикл для заповнення DataGridView квитками
            {
                dgvTickets.Rows.Add(
                    t.GetTypeName(),
                    t.SeatLabel,
                    t.IsAvailable ? "Доступний" : "Продано",
                    $"{t.GetPrice()} грн"
                );
            }
        }

        private void BtnBuy_Click(object? sender, EventArgs e)
        {
            if (dgvTickets.SelectedRows.Count == 0)
            {
                MessageBox.Show("Спочатку виберіть квиток.","Увага",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            int index = dgvTickets.SelectedRows[0].Index;
            Ticket selectedTicket = _concert.Seats[index];

            if (!selectedTicket.IsAvailable)
            {
                MessageBox.Show("Цей квиток вже проданий.","Помилка",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            using var form = new PurchaseForm(_concert,selectedTicket,_currentUserEmail);

            form.ShowDialog(this);
            LoadData();
        }
    }

}

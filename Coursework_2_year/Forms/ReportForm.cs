using Coursework_2_year.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Forms
{
    public class ReportForm : Form
    {
        private Label lblRevenue = null!;
        private DataGridView dgvByType = null!;
        private DataGridView dgvOrders = null!;
        private Button btnClose = null!;

        public ReportForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            Text = "Звіт доходів";
            ClientSize = new Size(806, 596);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10f);
            BackColor = SystemColors.Control;

            lblRevenue = new Label
            {
                Location = new Point(12, 12),
                Size = new Size(782, 28),
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            };

            dgvByType = BuildGrid(new Point(12, 48), new Size(782, 120));
            dgvByType.Columns.AddRange(
                new DataGridViewTextBoxColumn { Name = "colType", HeaderText = "Тип квитка" },
                new DataGridViewTextBoxColumn { Name = "colRev", HeaderText = "Дохід (грн)" },
                new DataGridViewTextBoxColumn { Name = "colCount", HeaderText = "Замовлень" }
            );

            dgvOrders = BuildGrid(new Point(12, 180), new Size(782, 356));
            dgvOrders.Columns.AddRange(
                new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "#" },
                new DataGridViewTextBoxColumn { Name = "colCustomer", HeaderText = "Клієнт" },
                new DataGridViewTextBoxColumn { Name = "colSeat", HeaderText = "Місце" },
                new DataGridViewTextBoxColumn { Name = "colType", HeaderText = "Тип" },
                new DataGridViewTextBoxColumn { Name = "colPrice", HeaderText = "Сума (грн)" },
                new DataGridViewTextBoxColumn { Name = "colTime", HeaderText = "Час" }
            );

            btnClose = new Button
            {
                Text = "Закрити",
                Location = new Point(668, 548),
                Size = new Size(126, 36),
            };
            btnClose.Click += (s, e) => Close();

            Controls.AddRange(new Control[] { lblRevenue, dgvByType, dgvOrders, btnClose });
        }

        private static DataGridView BuildGrid(Point location, Size size) => new()
        {
            Location = location,
            Size = size,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
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

        private void LoadData()
        {
            var sys = TicketingSystem.Instance;
            var orders = sys.GetAllOrders();
            var rev = sys.GetRevenueByType();

            lblRevenue.Text = $"Загальний дохід: {sys.GetTotalRevenue()} грн";

            dgvByType.Rows.Clear();
            foreach (var (type, amount) in rev)
            {
                int count = orders.Count(o => o.Ticket.GetTypeName() == type);
                dgvByType.Rows.Add(type, amount, count);
            }

            dgvOrders.Rows.Clear();
            foreach (var o in orders)
            {
                dgvOrders.Rows.Add(
                    o.Id,
                    o.Customer.Name,
                    o.Ticket.SeatLabel,
                    o.Ticket.GetTypeName(),
                    o.FinalPrice,
                    o.OrderTime.ToString("dd.MM.yyyy HH:mm")
                );
            }
        }
    }
}

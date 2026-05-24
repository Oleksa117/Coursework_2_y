using Coursework_2_year.Data;
using Coursework_2_year.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_year.Forms
{
    public class PurchaseForm : Form
    {
        private readonly Concert _concert;

        private ComboBox cmbType = null!;
        private ComboBox cmbCustomer = null!;
        private Panel pnlGuest = null!;
        private TextBox txtName = null!;
        private TextBox txtContact = null!;
        private Label lblPrice = null!;
        private Button btnConfirm = null!;
        private Button btnCancel = null!;
        private Label lblTicketType = null!;
        private TextBox txtSurname = null!;
        private Ticket? _selectedTicket;

        private readonly string _userEmail;

        public PurchaseForm(Concert concert, Ticket selectedTicket, string userEmail)
        {
            _concert = concert;
            _selectedTicket = selectedTicket;
            _userEmail = userEmail;

            InitializeComponent();

            LoadTicket();
            LoadCustomers();
        }

        private void InitializeComponent()
        {
            Text = "Купити квиток";
            ClientSize = new Size(450, 296);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10f);
            BackColor = SystemColors.Control;

            var lblType = new Label { Text = "Тип квитка:", Location = new Point(12, 18), Size = new Size(126, 22) };
            cmbType = new ComboBox
            {
                Location = new Point(146, 15),
                Size = new Size(292, 24),
                DropDownStyle = ComboBoxStyle.DropDownList,
            };

            cmbType.Visible = false;
            lblType.Visible = false;

            cmbType.Items.AddRange(new object[] { "VIP", "Standard", "Standing" });
            cmbType.SelectedIndex = 0;
            cmbType.SelectedIndexChanged += (s, e) => UpdatePrice();

            lblTicketType = new Label
            {
                Location = new Point(12, 18),
                Size = new Size(300, 24),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold)
            };

            var lblCustomer = new Label { Text = "Клієнт:", Location = new Point(12, 56), Size = new Size(126, 22) };
            lblCustomer.Visible = false;
            cmbCustomer = new ComboBox
            {
                Location = new Point(146, 53),
                Size = new Size(292, 24),
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            cmbCustomer.SelectedIndexChanged += CmbCustomer_Changed;

            cmbCustomer.Visible = false;
            lblCustomer.Visible = false;

            pnlGuest = new Panel
            {
                Location = new Point(12, 60),
                Size = new Size(426, 90),
                Visible = true
            };

            var lblGuestName = new Label
            {
                Text = "Ім'я:",
                Location = new Point(0, 4),
                Size = new Size(120, 22)
            };

            var lblGuestSurname = new Label
            {
                Text = "Прізвище:",
                Location = new Point(0, 42),
                Size = new Size(120, 22)
            };

            txtName = new TextBox
            {
                Location = new Point(134, 2),
                Size = new Size(290, 24)
            };

            txtSurname = new TextBox
            {
                Location = new Point(134, 40),
                Size = new Size(290, 24)
            };

            pnlGuest.Controls.AddRange(new Control[]
            {
                lblGuestName,
                txtName,
                lblGuestSurname,
                txtSurname
            });

            var lblPriceCaption = new Label { Text = "До сплати:", Location = new Point(12, 184), Size = new Size(126, 24) };
            lblPrice = new Label
            {
                Location = new Point(146, 184),
                Size = new Size(292, 24),
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = SystemColors.HotTrack,
            };

            btnConfirm = new Button
            {
                Text = "Підтвердити",
                Location = new Point(146, 244),
                Size = new Size(140, 36),
            };
            btnConfirm.Click += BtnConfirm_Click;

            btnCancel = new Button
            {
                Text = "Скасувати",
                Location = new Point(298, 244),
                Size = new Size(140, 36),
            };
            btnCancel.Click += (s, e) => Close();

            Controls.AddRange(new Control[]
            {
                lblTicketType,
                pnlGuest,
                lblPriceCaption,
                lblPrice,
                btnConfirm,
                btnCancel
            });
        }

        private void LoadCustomers()
        {
            cmbCustomer.Items.Clear();

            if (!string.IsNullOrWhiteSpace(_userEmail))
            {
                var user = TicketingSystem.Instance.GetUserByEmail(_userEmail);

                if (user != null)
                {
                    txtName.Text = user.FirstName;
                    txtSurname.Text = user.LastName;

                    txtName.ReadOnly = true;
                    txtSurname.ReadOnly = true;

                    return;
                }
            }

            txtName.Clear();
            txtSurname.Clear();

            txtName.ReadOnly = false;
            txtSurname.ReadOnly = false;
        }

        private void LoadTicket()
        {
            if (_selectedTicket == null)
            {
                MessageBox.Show("Квиток не вибраний.","Помилка",MessageBoxButtons.OK,MessageBoxIcon.Warning);

                Close();
                return;
            }

            lblTicketType.Text =$"Тип квитка: {_selectedTicket.GetTypeName()}";

            decimal price = _selectedTicket.GetPrice();

            if (!string.IsNullOrWhiteSpace(_userEmail))
            {
                price *= 0.9m;
            }

            lblPrice.Text = $"{price:F0} грн (знижка -10%)";
        }

        private void CmbCustomer_Changed(object? sender, EventArgs e)
        {
            pnlGuest.Visible = cmbCustomer.SelectedItem is string s && s == "Новий гість";
            UpdatePrice();
        }

        private void UpdatePrice()
        {
            string type = "VIP";
            decimal basePrice = type switch
            {
                "VIP" => 2000m,
                "Standard" => 800m,
                _ => 300m,
            };

            bool isRegistered = cmbCustomer.SelectedItem is RegisteredCustomer;
            decimal final = isRegistered ? basePrice * 0.9m : basePrice;
            string note = isRegistered ? " (-10% знижка)" : "";

            lblPrice.Text = $"{final} грн{note}";
        }

        private void BtnConfirm_Click(object? sender, EventArgs e)
        {
            string type = _selectedTicket?.GetTypeName() ?? "VIP";

            Customer? customer;
            if (cmbCustomer.SelectedItem is Customer c)
            {
                customer = c;
            }
            else
            {
                string name = txtName.Text.Trim();
                string surname = txtSurname.Text.Trim();

                if (string.IsNullOrWhiteSpace(name) ||
                    string.IsNullOrWhiteSpace(surname))
                {
                    MessageBox.Show("Введіть ім'я та прізвище.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                customer = new GuestCustomer
                {
                    Name = $"{name} {surname}",
                    ContactInfo = ""
                };
            }

            var order = TicketingSystem.Instance.PurchaseTicket(_concert, type, customer);
            if (order == null)
            {
                MessageBox.Show(
                    $"На жаль, квитки типу «{type}» на цей концерт закінчились.",
                    "Немає квитків",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(order.GetReceiptText(), "Успішна покупка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}

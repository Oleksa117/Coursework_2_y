using System;
using System.Windows.Forms;
using Coursework_2_year.Data;
using Coursework_2_year.Models;
using System.Text.RegularExpressions;

namespace Coursework_2_year.Forms
{
    public class RegisterForm : Form
    {
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtEmail;
        private TextBox txtPassword;

        private Button btnRegister;

        public RegisterForm()
        {
            Text = "Реєстрація";
            Width = 400;
            Height = 350;

            Label lblFirstName = new()
            {
                Text = "Ім'я:",
                Left = 20,
                Top = 20,
                Width = 100
            };

            txtFirstName = new()
            {
                Left = 130,
                Top = 20,
                Width = 200
            };

            Label lblLastName = new()
            {
                Text = "Прізвище:",
                Left = 20,
                Top = 60,
                Width = 100
            };

            txtLastName = new()
            {
                Left = 130,
                Top = 60,
                Width = 200
            };

            Label lblEmail = new()
            {
                Text = "Email:",
                Left = 20,
                Top = 100,
                Width = 100
            };

            txtEmail = new()
            {
                Left = 130,
                Top = 100,
                Width = 200
            };

            Label lblPassword = new()
            {
                Text = "Пароль:",
                Left = 20,
                Top = 140,
                Width = 100
            };

            txtPassword = new()
            {
                Left = 130,
                Top = 140,
                Width = 200,
                PasswordChar = '*'
            };

            btnRegister = new()
            {
                Text = "Зареєструватися",
                Left = 150,
                Top = 200,
                Width = 140
            };

            btnRegister.Click += BtnRegister_Click;

            Controls.AddRange(new Control[]
            {
                lblFirstName, txtFirstName,
                lblLastName, txtLastName,
                lblEmail, txtEmail,
                lblPassword, txtPassword,
                btnRegister
            });
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Заповніть усі поля","Попередження",MessageBoxButtons.OK,MessageBoxIcon.Warning);

                return;
            }

            User user = new()
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                Role = "Customer"
            };

            UserRepository.Register(user);

            MessageBox.Show("Реєстрацію успішно завершено!","Інформація",MessageBoxButtons.OK,MessageBoxIcon.Information);

            Close();
        }
    }
}
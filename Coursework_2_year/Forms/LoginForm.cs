using System;
using System.Windows.Forms;
using Coursework_2_year.Data;
using Coursework_2_year.Models;

namespace Coursework_2_year.Forms
{
    public class LoginForm : Form
    {
        private Label lblEmail;
        private Label lblPassword;

        private TextBox txtEmail;
        private TextBox txtPassword;

        private Button btnLogin;
        private Button btnRegister;

        public LoginForm()
        {
            Text = "Авторизація";
            Width = 400;
            Height = 320;
            StartPosition = FormStartPosition.CenterScreen;

            lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Left = 30;
            lblEmail.Top = 35;
            lblEmail.Width = 80;

            txtEmail = new TextBox();
            txtEmail.Left = 120;
            txtEmail.Top = 30;
            txtEmail.Width = 200;

            lblPassword = new Label();
            lblPassword.Text = "Пароль:";
            lblPassword.Left = 30;
            lblPassword.Top = 85;
            lblPassword.Width = 80;

            txtPassword = new TextBox();
            txtPassword.Left = 120;
            txtPassword.Top = 80;
            txtPassword.Width = 200;
            txtPassword.PasswordChar = '*';

            btnLogin = new Button();
            btnLogin.Text = "Увійти";
            btnLogin.SetBounds(60, 180, 140, 35);
            btnLogin.Click += BtnLogin_Click;

            btnRegister = new Button();
            btnRegister.Text = "Зареєструватися";
            btnRegister.SetBounds(220, 180, 140, 35);
            btnRegister.Click += BtnRegister_Click;

            Controls.Add(lblEmail);
            Controls.Add(txtEmail);

            Controls.Add(lblPassword);
            Controls.Add(txtPassword);

            Controls.Add(btnLogin);
            Controls.Add(btnRegister);
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                 string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Будь ласка, введіть email та пароль", "Попередження",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User user = UserRepository.Login(txtEmail.Text, txtPassword.Text);

            if (user is null)  
            {
                MessageBox.Show("Невірний email або пароль", "Помилка",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Hide();

            MainForm form = new MainForm();
            form.ShowDialog();

            Close();
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }
    }
}
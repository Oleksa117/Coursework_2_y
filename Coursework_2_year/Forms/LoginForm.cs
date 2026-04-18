using System;
using System.Windows.Forms;

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
            Text = "Login";
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
            lblPassword.Text = "Password:";
            lblPassword.Left = 30;
            lblPassword.Top = 85;
            lblPassword.Width = 80;

            txtPassword = new TextBox();
            txtPassword.Left = 120;
            txtPassword.Top = 80;
            txtPassword.Width = 200;
            txtPassword.PasswordChar = '*';

            btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.SetBounds(60, 180, 140, 35);

            btnRegister = new Button();
            btnRegister.Text = "Register";
            btnRegister.SetBounds(220, 180, 140, 35);

            Controls.Add(lblEmail);
            Controls.Add(txtEmail);

            Controls.Add(lblPassword);
            Controls.Add(txtPassword);

            Controls.Add(btnLogin);
            Controls.Add(btnRegister);
        }
    }
}
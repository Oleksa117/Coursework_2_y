using Coursework_2_year.Data;
using System.Text.RegularExpressions;
using Coursework_2_year.Models;

namespace Coursework_2_year.Forms;

public class LoginForm : Form
{
    public string SelectedRole { get; private set; } = "Guest";
    public string UserEmail => txtEmail.Text;

    private RadioButton rbAdmin;
    private RadioButton rbClient;
    private RadioButton rbGuest;

    private TextBox txtEmail;
    private TextBox txtPassword;

    private Label lblEmail;
    private Label lblPassword;
    private Label lblGuestInfo;

    private Button btnRegister;

    public LoginForm()
    {
        Text = "Вхід";
        ClientSize = new Size(500, 320);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        rbAdmin = new RadioButton()
        {
            Text = "Адміністратор",
            Location = new Point(20, 20),
            AutoSize = true
        };

        rbClient = new RadioButton()
        {
            Text = "Клієнт",
            Location = new Point(20, 50),
            AutoSize = true
        };

        rbGuest = new RadioButton()
        {
            Text = "Гість",
            Location = new Point(20, 80),
            AutoSize = true,
            Checked = true
        };

        rbGuest.CheckedChanged += RoleChanged;
        rbClient.CheckedChanged += RoleChanged;
        rbAdmin.CheckedChanged += RoleChanged;

        lblGuestInfo = new Label()
        {
            Text = "Вхід можливий без реєстрації",
            Location = new Point(20, 120),
            AutoSize = true
        };

        lblEmail = new Label()
        {
            Text = "Email:",
            Location = new Point(20, 100),
            AutoSize = true
        };

        txtEmail = new TextBox()
        {
            Location = new Point(20, 120),
            Width = 250
        };

        lblPassword = new Label()
        {
            Text = "Пароль:",
            Location = new Point(20, 150),
            AutoSize = true
        };

        txtPassword = new TextBox()
        {
            Location = new Point(20, 170),
            Width = 250,
            PasswordChar = '*'
        };

        Button btnOk = new Button()
        {
            Text = "Увійти",
            Location = new Point(20, 240),
            Size = new Size(150, 35)
        };

        btnOk.Click += BtnOk_Click;

        btnRegister = new Button()
        {
            Text = "Реєстрація",
            Location = new Point(200, 240),
            Size = new Size(150, 35)
        };

        btnRegister.Click += BtnRegister_Click;
        // Спочатку додаємо всі елементи, а потім оновлюємо видимість
        Controls.AddRange(
        [
            rbAdmin,
            rbClient,
            rbGuest,

            lblGuestInfo,

            lblEmail,
            txtEmail,

            lblPassword,
            txtPassword,

            btnRegister,
            btnOk
        ]);

        RoleChanged(null, EventArgs.Empty);
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        if (rbGuest.Checked)// Вхід як гість не вимагає перевірки даних
        {
            SelectedRole = "Guest";
            DialogResult = DialogResult.OK;
            return;
        }

        if (!Regex.IsMatch(txtEmail.Text,@"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            MessageBox.Show("Невірний Email","Помилка",MessageBoxButtons.OK,MessageBoxIcon.Error);

            return;
        }

        if (rbAdmin.Checked)
        {
            if (txtEmail.Text == "admin@gmail.com" &&
                txtPassword.Text == "Admin123")
            {
                SelectedRole = "Admin";
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Невірний логін або пароль","Помилка",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

            return;
        }
        // Вхід як клієнт вимагає перевірки даних через систему
        if (rbClient.Checked)
        {
            var role = TicketingSystem.Instance.Login(txtEmail.Text,txtPassword.Text);
           
            if (role != null)
            {
                SelectedRole = role;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Невірний логін або пароль","Помилка",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }

    private void BtnRegister_Click(object? sender, EventArgs e)
    {
        using var form = new RegisterForm();
        form.ShowDialog(this);
    }

    private void RoleChanged(object? sender, EventArgs e)
    {
        bool needLogin =rbClient.Checked || rbAdmin.Checked;

        lblEmail.Visible = needLogin;
        txtEmail.Visible = needLogin;

        lblPassword.Visible = needLogin;
        txtPassword.Visible = needLogin;

        lblGuestInfo.Visible = rbGuest.Checked;

        btnRegister.Visible = rbGuest.Checked;
    }
}
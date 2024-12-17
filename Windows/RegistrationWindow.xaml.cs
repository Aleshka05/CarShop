using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using BCrypt.Net;
using CarShop.Model;
namespace CarShopApp
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var userName = txtUserName.Text.Trim();
            var userEmail = txtUserEmail.Text.Trim();
            var userPhone = txtUserPhone.Text.Trim();
            var userLogin = txtUserLogin.Text.Trim();
            var userPassword = txtUserPassword.Password.Trim();

            if (!ValidateFields(userName, userEmail, userPhone, userLogin, userPassword))
            {
                return;
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userPassword);

            using (var context = new CarShopContext())
            {

                var existingUser = context.Users.FirstOrDefault(u => u.Login == userLogin);
                if (existingUser != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var existingEmail = context.Users.FirstOrDefault(u => u.Email == userEmail);
                if (existingEmail != null)
                {
                    MessageBox.Show("Пользователь с таким email уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var existingPhone = context.Users.FirstOrDefault(u => u.Phone == userPhone);
                if (existingPhone != null)
                {
                    MessageBox.Show("Пользователь с таким номером телефона уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var clientRole = context.Roles.FirstOrDefault(r => r.Name == "Клиент");

                var newUser = new User
                {
                    UserName = userName,
                    Email = userEmail,
                    Phone = userPhone,
                    Login = userLogin,
                    Password = hashedPassword,
                    RoleId = clientRole?.Id ?? 2
                };

                context.Users.Add(newUser);
                context.SaveChanges();
            }

            MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }


        private bool ValidateFields(string name, string email, string phone, string login, string password)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!Regex.IsMatch(login, @"[a-zA-Z]"))
            {
                MessageBox.Show("Логин должен содержать хотя бы одну букву.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Неправильный формат email.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!Regex.IsMatch(phone, @"^\d{10,15}$"))
            {
                MessageBox.Show("Телефон должен содержать только цифры и быть длиной от 10 до 15 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$"))
            {
                MessageBox.Show("Пароль должен быть не менее 6 символов, содержать цифры, буквы верхнего и нижнего регистра.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

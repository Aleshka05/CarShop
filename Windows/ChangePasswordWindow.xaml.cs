using System;
using System.Linq;
using System.Windows;
using CarShop.Model;
namespace CarShopApp
{
    public partial class ChangePasswordWindow : Window
    {
        private User CurrentUser;

        public ChangePasswordWindow(User user)
        {
            InitializeComponent();
            CurrentUser = user ?? throw new ArgumentNullException(nameof(user)); 
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = CurrentPasswordBox.Password;
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, CurrentUser.Password))
            {
                MessageBox.Show("Текущий пароль неверен!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Новый пароль и подтверждение не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(newPassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$"))
            {
                MessageBox.Show("Пароль должен быть не менее 6 символов, содержать цифры, буквы верхнего и нижнего регистра.",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            try
            {
                using (var db = new CarShopContext())
                {
                    // Поиск пользователя в базе данных
                    var userToUpdate = db.Users.FirstOrDefault(u => u.Id == CurrentUser.Id);
                    if (userToUpdate != null)
                    {
                        userToUpdate.Password = hashedPassword;
                        db.SaveChanges();

                        MessageBox.Show("Пароль успешно изменён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении пароля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

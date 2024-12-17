using System;
using System.Windows;
using System.Windows.Controls;
using CarShop.Model;
namespace CarShopApp
{
    public partial class UserPage : Page
    {
        private User CurrentUser;

        public UserPage(User user)
        {
            InitializeComponent();
            CurrentUser = user; 
            LoadUserData();
        }

        
        private void LoadUserData()
        {
            if (CurrentUser != null)
            {
                try
                {
                    UserNameTextBlock.Text = CurrentUser.UserName ?? "Не указано";
                    UserEmailTextBlock.Text = CurrentUser.Email ?? "Не указано";
                    UserPhoneTextBlock.Text = CurrentUser.Phone ?? "Не указано";
                    UserLoginTextBlock.Text = CurrentUser.Login ?? "Не указано";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Текущий пользователь не задан.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы вышли из аккаунта.", "Выход", MessageBoxButton.OK, MessageBoxImage.Information);
            var loginWindow = new MainWindow();
            loginWindow.Show();
            Window.GetWindow(this)?.Close();
        }
        private void OpenChangePasswordWindow_Click(object sender, RoutedEventArgs e)
        {
           
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(CurrentUser);
            changePasswordWindow.ShowDialog();
        }

    }
}

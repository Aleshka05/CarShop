using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;

namespace CarShopApp
{
    public partial class MainWindow : Window
    {
    
        public MainWindow()
        {
            InitializeComponent();

           
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Password;

            using (var context = new CarShopContext())
            {
                
                var user = context.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Login == username);

                if (user != null)
                {
                    if (user.Role != null)
                    {
                        
                        if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                        {
                            MessageBox.Show("Вход выполнен успешно!");

                           
                            var mainPage = new CarShopWindow(user);
                            mainPage.Show();

                            this.Close(); 
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Роль пользователя не найдена.");
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не найден.");
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
           
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close(); 
        }

    }

}

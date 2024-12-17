using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;
using CarShop.Model;
using System;

namespace CarShopApp
{
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            using (var context = new CarShopContext())
            {
                var users = context.Users.Include(u => u.Role)
                    .Select(u => new
                    {
                        u.Id,
                        u.UserName,
                        u.Email,
                        u.Phone,
                        u.Login,
                        RoleName = u.Role.Name
                    }).ToList();

                UsersDataGrid.ItemsSource = users;
            }
        }

        private void UsersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var editedUser = e.Row.Item as User; 
            if (editedUser == null) return;

            try
            {
                using (var context = new CarShopContext())
                {
                    var user = context.Users.FirstOrDefault(u => u.Id == editedUser.Id);

                    if (user != null)
                    {
                        user.UserName = editedUser.UserName;
                        user.Email = editedUser.Email;
                        user.Phone = editedUser.Phone;
                        user.Login = editedUser.Login;

                        context.SaveChanges();
                        MessageBox.Show("Данные пользователя успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

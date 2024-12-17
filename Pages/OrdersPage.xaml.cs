using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using CarShop.Model;
namespace CarShopApp
{
    public partial class OrdersPage : Page
    {
        private readonly User _currentUser;
        private List<Order> _allOrders;
        private int _currentPage = 1;
        private int _itemsPerPage = 10;     
        private int _totalPages;            


        public OrdersPage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadOrders();
        }

        private async void LoadOrders(int page = 1)
        {
            _currentPage = page; 

            using (var context = new CarShopContext())
            {
                IQueryable<Order> query;

               
                if (_currentUser.Role.Name == "Администратор")
                {
                    query = context.Orders
                        .Include(o => o.User)
                        .Join(context.Cars,
                            order => order.CarId,
                            car => car.CarID,
                            (order, car) => new Order
                            {
                                OrderId = order.OrderId,
                                CarModel = car.CarModel,
                                OrderDate = order.OrderDate,
                                Status = order.Status,
                                User = order.User
                            });
                }
                else
                {
                    query = context.Orders
                        .Where(o => o.UserId == _currentUser.Id)
                        .Include(o => o.User)
                        .Join(context.Cars,
                            order => order.CarId,
                            car => car.CarID,
                            (order, car) => new Order
                            {
                                OrderId = order.OrderId,
                                CarModel = car.CarModel,
                                OrderDate = order.OrderDate,
                                Status = order.Status,
                                User = order.User
                            });
                }

                
                int totalRecords = await query.CountAsync();
                _totalPages = (int)Math.Ceiling((double)totalRecords / _itemsPerPage);            
                _allOrders = await query
                    .OrderBy(o => o.OrderId)
                    .Skip((page - 1) * _itemsPerPage) 
                    .Take(_itemsPerPage)             
                    .ToListAsync();

                DisplayOrders(_allOrders);

                UpdatePaginationButtons();
            }
        }
        private void UpdatePaginationButtons()
        {
            PaginationPanel.Children.Clear();

            Button previousButton = new Button
            {
                Content = "Назад",
                Width = 100,
                Margin = new Thickness(5),
                IsEnabled = _currentPage > 1
            };
            previousButton.Click += (s, e) => LoadOrders(_currentPage - 1);

            TextBlock pageInfo = new TextBlock
            {
                Text = $"Страница {_currentPage} из {_totalPages}",
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10)
            };

            Button nextButton = new Button
            {
                Content = "Вперёд",
                Width = 100,
                Margin = new Thickness(5),
                IsEnabled = _currentPage < _totalPages
            };
            nextButton.Click += (s, e) => LoadOrders(_currentPage + 1);

            PaginationPanel.Children.Add(previousButton);
            PaginationPanel.Children.Add(pageInfo);
            PaginationPanel.Children.Add(nextButton);
        }


        private void DisplayOrders(List<Order> orders)
        {
            OrdersStackPanel.Children.Clear();

            foreach (var order in orders)
            {
                StackPanel orderPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10),
                    Background = System.Windows.Media.Brushes.White,
                };

                string orderDescription = $"{order.CarModel} - {order.OrderDate:dd.MM.yyyy} - {order.Status}";

                if (_currentUser.Role.Name == "Администратор")
                {
                    orderDescription += $" | Заказчик: {order.User?.UserName ?? "Неизвестный"}";
                }

                TextBlock orderTextBlock = new TextBlock
                {
                    Text = orderDescription,
                    Foreground = System.Windows.Media.Brushes.Black,
                    FontSize = 16,
                    Margin = new Thickness(10),
                    VerticalAlignment = VerticalAlignment.Center
                };

                orderPanel.Children.Add(orderTextBlock);

                if (_currentUser.Role.Name == "Администратор")
                {
                    ComboBox statusComboBox = new ComboBox
                    {
                        Width = 150,
                        Margin = new Thickness(10),
                        SelectedValue = order.Status
                    };

                    statusComboBox.Items.Add("В обработке");
                    statusComboBox.Items.Add("На погрузке");
                    statusComboBox.Items.Add("В пути");
                    statusComboBox.Items.Add("Доставлен");

                    statusComboBox.SelectionChanged += (sender, e) =>
                    {
                        var selectedComboBox = sender as ComboBox;
                        order.Status = selectedComboBox.SelectedItem.ToString();
                    };

                    orderPanel.Children.Add(statusComboBox);

                    Button changeStatusButton = new Button
                    {
                        Content = "Изменить",
                        Width = 100,
                        Height = 30,
                        Margin = new Thickness(10),
                        Tag = order.OrderId
                    };

                    changeStatusButton.Click += async (sender, e) =>
                    {
                        int orderId = (int)((Button)sender).Tag;

                        using (var context = new CarShopContext())
                        {
                            var orderToUpdate = await context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
                            if (orderToUpdate != null)
                            {
                                orderToUpdate.Status = order.Status;
                                await context.SaveChangesAsync();
                                MessageBox.Show("Статус заказа изменен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                                await SendEmailNotification(orderToUpdate);

                                LoadOrders();
                            }
                        }
                    };

                    orderPanel.Children.Add(changeStatusButton);

                    Button deleteOrderButton = new Button
                    {
                        Content = "Удалить заказ",
                        Width = 120,
                        Height = 30,
                        Margin = new Thickness(10),
                        Tag = order.OrderId
                    };

                    deleteOrderButton.Click += async (sender, e) =>
                    {
                        int orderId = (int)((Button)sender).Tag;

                        if (MessageBox.Show("Вы уверены, что хотите удалить этот заказ?", "Подтверждение удаления",
                            MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            await DeleteOrder(orderId);
                            LoadOrders();
                        }
                    };

                    orderPanel.Children.Add(deleteOrderButton);
                }

                OrdersStackPanel.Children.Add(orderPanel);
            }
        }



        private async Task DeleteOrder(int orderId)
        {
            using (var context = new CarShopContext())
            {
                var orderToDelete = await context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
                if (orderToDelete != null)
                {
                    context.Orders.Remove(orderToDelete);
                    await context.SaveChangesAsync();
                    MessageBox.Show("Заказ успешно удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось найти заказ для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private async Task SendEmailNotification(Order order)
        {
            try
            {
                if (order == null)
                {
                    MessageBox.Show("Заказ не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (var context = new CarShopContext())
                {
                    var user = await context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId);
                    if (user == null || string.IsNullOrEmpty(user.Email))
                    {
                        MessageBox.Show("Email пользователя не задан.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var fromAddress = new MailAddress("Savchuk017@gmail.com", "Car Shop");
                    var toAddress = user.Email; 

                    const string subject = "Изменение статуса заказа";
                    string body = $"Здравствуйте,\n\nСтатус вашего заказа на автомобиль {order.CarModel} был изменен на: {order.Status}.\n\nС уважением,\nCar Shop";

                    using (var smtp = new SmtpClient("smtp.gmail.com"))
                    {
                        smtp.Port = 587;  
                        smtp.Credentials = new NetworkCredential("Savchuk017@gmail.com", "wadz cmcg mzgu fqvq"); 
                        smtp.EnableSsl = true;  

                        await smtp.SendMailAsync(fromAddress.Address, toAddress, subject, body); 
                    }

                    MessageBox.Show("Уведомление о изменении статуса отправлено на почту.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке уведомления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchTextBox.Text.ToLower();
            var startDate = StartDatePicker.SelectedDate;
            var endDate = EndDatePicker.SelectedDate;

            string selectedStatus = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            var filteredOrders = _allOrders.AsEnumerable();

            if (!string.IsNullOrEmpty(searchText))
            {
                filteredOrders = filteredOrders.Where(o => o.CarModel.ToLower().Contains(searchText) || o.Status.ToLower().Contains(searchText));
            }

            if (startDate.HasValue)
            {
                filteredOrders = filteredOrders.Where(o => o.OrderDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                filteredOrders = filteredOrders.Where(o => o.OrderDate <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(selectedStatus))
            {
                filteredOrders = filteredOrders.Where(o => o.Status == selectedStatus);
            }

            DisplayOrders(filteredOrders.ToList());
        }



    }
}

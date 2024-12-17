using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using CarShop.Model;
namespace CarShopApp
{
    public partial class CarShopWindow : Window
    {
        private readonly User _currentUser;

        public CarShopWindow(User user)
        {
            InitializeComponent();
           
            _currentUser = user; 
            UpdateNavigationMenu();
            GoToMainPage(null, null);

        }

       
        private void UpdateNavigationMenu()
        {
            
            if (_currentUser.Role.Name == "Администратор")
            {
                
                Button addCarButton = new Button
                {
                    Content = "Добавить автомобиль",
                    Height = 50,
                    Foreground = System.Windows.Media.Brushes.White,
                    Background = System.Windows.Media.Brushes.Transparent,
                    BorderBrush = System.Windows.Media.Brushes.Transparent,
                    FontSize = 16,
                    Margin = new System.Windows.Thickness(10)
                };
                addCarButton.Click += AddCarButton_Click;
                NavigationPanel.Children.Add(addCarButton);

                Button usersButton = new Button
                {
                    Content = "Пользователи",
                    Height = 50,
                    Foreground = System.Windows.Media.Brushes.White,
                    Background = System.Windows.Media.Brushes.Transparent,
                    BorderBrush = System.Windows.Media.Brushes.Transparent,
                    FontSize = 16,
                    Margin = new System.Windows.Thickness(10)
                };
                usersButton.Click += UsersButton_Click;
                NavigationPanel.Children.Add(usersButton);
            }
        }

       
        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new AddCarPage();
            MainContent.Visibility = Visibility.Visible;
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UsersPage();
            MainContent.Visibility = Visibility.Visible;
        }

      
        private void GoToMainPage(object sender, RoutedEventArgs e)
        {
            var mainPage = new MainPage(_currentUser); 
            MainContent.Content = mainPage;
            MainContent.Visibility = Visibility.Visible;
        }

       
        private void GoToAboutUsPage(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new AboutUsPage();
            MainContent.Visibility = Visibility.Visible;
        }

       

        private void GoToUserPage(object sender, RoutedEventArgs e)
        {
            var userPage = new UserPage(_currentUser); 
            MainContent.Content = userPage;          
            MainContent.Visibility = Visibility.Visible;
        }

        private void GoToOrderPage(object sender, RoutedEventArgs e)
        {
            var ordersPage = new OrdersPage(_currentUser);
            MainContent.Content = ordersPage;
            MainContent.Visibility = Visibility.Visible;
        }

        private void GoToReviewsPage(object sender, RoutedEventArgs e)
        {
            var reviewsPage = new ReviewsPage(_currentUser);
            MainContent.Content = reviewsPage;
            MainContent.Visibility = Visibility.Visible;
        }
    }
}

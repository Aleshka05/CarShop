using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CarShop.Model;

namespace CarShopApp
{
    public partial class CarDetailsPage : Page
    {
        private readonly int _carId;
        private readonly User _currentUser;

        public CarDetailsPage(int carId, User user)
        {
            InitializeComponent();
            _carId = carId;
            _currentUser = user;
            LoadCarDetails();
        }

        private void LoadCarDetails()
        {
            using (var context = new CarShopContext())
            {
                var car = context.Cars
                    .Where(c => c.CarID == _carId)
                    .Select(c => new
                    {
                        c.CarModel,
                        c.CarType,
                        c.TransmissionType,
                        c.CarPrice,
                        c.CarImage,
                        c.CarDesc,
                        c.Year,                
                        c.Mileage,            
                        c.Color,             
                        c.DriveType,        
                    
                        c.Configuration.Salon,
                        c.Configuration.SafetySystems,
                        c.Configuration.Airbags,
                        c.Configuration.AssistanceSystems,
                        c.Configuration.Exterior,
                        c.Configuration.Multimedia
                    })
                    .FirstOrDefault();

                if (car != null)
                {
                    try
                    {
                       
                        CarImage.Source = new BitmapImage(new Uri(car.CarImage, UriKind.RelativeOrAbsolute));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
                    }

                   
                    CarModelTextBlock.Text = $"Модель: {car.CarModel}";
                    CarTypeTextBlock.Text = $"Тип: {car.CarType}";
                    CarTransmissionTextBlock.Text = $"Коробка передач: {car.TransmissionType}";
                    CarPriceTextBlock.Text = $"Цена: ${car.CarPrice}";
                    CarDescriptionTextBlock.Text = $"Описание: {car.CarDesc}";

                   
                    CarYearTextBlock.Text = $"Год выпуска: {car.Year}";
                    CarMileageTextBlock.Text = $"Пробег: {car.Mileage} км";
                    CarColorTextBlock.Text = $"Цвет: {car.Color}";
                    CarEngineTypeTextBlock.Text = $"Двигатель: {car.DriveType}";

                   
                    CarSalonTextBlock.Text = $"Салон: {car.Salon}";
                    CarSafetySystemsTextBlock.Text = $"Системы безопасности: {car.SafetySystems}";
                    CarAirbagsTextBlock.Text = $"Подушки безопасности: {car.Airbags}";
                    CarAssistanceSystemsTextBlock.Text = $"Системы помощи: {car.AssistanceSystems}";
                    CarExteriorTextBlock.Text = $"Экстерьер: {car.Exterior}";
                    CarMultimediaTextBlock.Text = $"Мультимедиа: {car.Multimedia}";

                   
                    if (_currentUser.Role.Name == "Администратор")
                    {
                        DeleteCarButton.Visibility = Visibility.Visible;
                        
                    }
                    else
                    {
                        DeleteCarButton.Visibility = Visibility.Collapsed;
                     
                    }
                }
                else
                {
                    MessageBox.Show("Автомобиль не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            var confirmationWindow = new OrderConfirmationWindow(_currentUser);
            confirmationWindow.ShowDialog();
           
            if (confirmationWindow.IsConfirmed)
            {
                using (var context = new CarShopContext())
                {
                    var car = context.Cars.FirstOrDefault(c => c.CarID == _carId);

                    if (car != null)
                    {
                        var newOrder = new Order
                        {
                            CarId = _carId,
                            UserId = _currentUser.Id,
                            OrderDate = DateTime.Now,
                            Status = "В обработке",
                            CarModel = car.CarModel
                        };

                        context.Orders.Add(newOrder);
                        context.SaveChanges();

                        MessageBox.Show("Ваш заказ принят! Мы свяжемся с вами для уточнения деталей.",
                                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Автомобиль не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Заказ не был подтверждён.", "Отмена", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void DeleteCarButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот автомобиль?", 
                "Удалить автомобиль", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new CarShopContext())
                {
                    var car = context.Cars.FirstOrDefault(c => c.CarID == _carId);

                    if (car != null)
                    {
                        context.Cars.Remove(car);
                        context.SaveChanges();
                        MessageBox.Show("Автомобиль был удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService?.GoBack();
                    }
                    else
                    {
                        MessageBox.Show("Автомобиль не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}

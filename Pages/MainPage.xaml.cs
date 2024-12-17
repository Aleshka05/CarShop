using CarShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CarShop.Model;
namespace CarShopApp
{
    public partial class MainPage : Page
    {
        private readonly User _currentUser;
        private List<Car> _allCars; 
        private int _currentPage = 1; 
        private const int _itemsPerPage = 10; 
        private int _totalPages; 

        public MainPage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadCars();
            LoadYearRange();
        }

        private void LoadCars()
        {
            var carService = new CarService(); 
            _allCars = carService.GetCars();  
            _totalPages = (int)Math.Ceiling((double)_allCars.Count / _itemsPerPage); 
            DisplayCars(_allCars); 
        }

        private void DisplayCars(List<Car> cars)
        {
            CarStackPanel.Children.Clear();

          
            var carsForCurrentPage = cars.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();

            foreach (var car in carsForCurrentPage)
            {
               
                StackPanel carPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };

               
                string carDescription = $"{car.CarModel}, {car.Year}, {car.TransmissionType}, {car.DriveType}, {car.Color}, {car.Mileage} км, {car.BodyType}";

               
                TextBlock carTextBlock = new TextBlock
                {
                    Text = carDescription,
                    FontSize = 16,
                    Margin = new Thickness(10),
                    HorizontalAlignment = HorizontalAlignment.Left
                };

               
                TextBlock priceTextBlock = new TextBlock
                {
                    Text = $"Цена: ${car.CarPrice}",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(10, 0, 10, 10),
                    HorizontalAlignment = HorizontalAlignment.Left
                };

             
                Button detailsButton = new Button
                {
                    Content = "Подробнее",
                    Width = 150,
                    Height = 40,
                    Margin = new Thickness(10),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Tag = car.CarID
                };
                detailsButton.Click += DetailsButton_Click;

               
                StackPanel carInfoPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10)
                };

              
                carInfoPanel.Children.Add(carTextBlock);
                carInfoPanel.Children.Add(priceTextBlock);
                carInfoPanel.Children.Add(detailsButton);

               
                if (!string.IsNullOrEmpty(car.CarImage))
                {
                    Image carImage = new Image
                    {
                        Source = new BitmapImage(new Uri(car.CarImage, UriKind.RelativeOrAbsolute)),
                        Width = 200,    
                        Height = 150,   
                        Stretch = Stretch.UniformToFill,  
                        Margin = new Thickness(10)
                    };

                    carPanel.Children.Add(carImage);
                }

                carPanel.Children.Add(carInfoPanel);
                CarStackPanel.Children.Add(carPanel);
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            int carId = (int)((Button)sender).Tag;

          
            NavigationService.Navigate(new CarDetailsPage(carId, _currentUser));
        }
        private void LoadYearRange()
        {
            int startYear = 1900;
            int endYear = 2024;

            for (int year = startYear; year <= endYear; year++)
            {
                YearMinComboBox.Items.Add(new ComboBoxItem { Content = year.ToString() });
                YearMaxComboBox.Items.Add(new ComboBoxItem { Content = year.ToString() });
            }
        }
        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                ApplyFilters();
            }
        }

       
        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                ApplyFilters();
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void PriceMinTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void PriceMaxTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void BodyTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void YearMinComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void YearMaxComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();
            decimal.TryParse(PriceMinTextBox.Text.Trim(), out decimal minPrice);
            decimal.TryParse(PriceMaxTextBox.Text.Trim(), out decimal maxPrice);

            int? minYear = null;
            int? maxYear = null;

            if (YearMinComboBox.SelectedItem is ComboBoxItem minItem && int.TryParse(minItem.Content.ToString(), out int selectedMinYear))
            {
                minYear = selectedMinYear;
            }

            if (YearMaxComboBox.SelectedItem is ComboBoxItem maxItem && int.TryParse(maxItem.Content.ToString(), out int selectedMaxYear))
            {
                maxYear = selectedMaxYear;
            }

           
            string selectedBodyType = (BodyTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

           
            string selectedDriveType = (DriveTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            
            var filteredCars = _allCars.Where(car =>
                (string.IsNullOrEmpty(searchText) || car.CarModel.ToLower().Contains(searchText)) &&
                (minPrice == 0 || car.CarPrice >= minPrice) &&
                (maxPrice == 0 || car.CarPrice <= maxPrice) &&
                (!minYear.HasValue || car.Year >= minYear.Value) &&
                (!maxYear.HasValue || car.Year <= maxYear.Value) &&
                (string.IsNullOrEmpty(selectedBodyType) || car.BodyType == selectedBodyType) &&
                (string.IsNullOrEmpty(selectedDriveType) || car.DriveType == selectedDriveType)) 
                .ToList();

            _totalPages = (int)Math.Ceiling((double)filteredCars.Count / _itemsPerPage); 
            DisplayCars(filteredCars);
        }

    }
}

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using CarShop.Model;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarShopApp
{
    public partial class AddCarPage : Page
    {
        public AddCarPage()
        {
            InitializeComponent();
        }

    
        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|Все файлы (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                txtCarImage.Text = openFileDialog.FileName;
            }
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
        
            if (e.Key == Key.Enter)
            {
                
                e.Handled = true;
            }
        }
       
        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
             
                string carModel = txtCarModel.Text.Trim();
                string carType = cmbCarType.SelectedItem != null
                    ? ((ComboBoxItem)cmbCarType.SelectedItem).Content.ToString()
                    : string.Empty;
                string transmissionType = cmbTransmissionType.SelectedItem != null
                    ? ((ComboBoxItem)cmbTransmissionType.SelectedItem).Content.ToString()
                    : string.Empty;
                string carDesc = txtCarDesc.Text.Trim();
                string carImage = txtCarImage.Text.Trim();
                string driveType = cmbDriveType.SelectedItem != null
                    ? ((ComboBoxItem)cmbDriveType.SelectedItem).Content.ToString()
                    : string.Empty;
                string color = txtColor.Text.Trim();
                string bodyType = cmbBodyType.SelectedItem != null
                    ? ((ComboBoxItem)cmbBodyType.SelectedItem).Content.ToString()
                    : string.Empty;

                decimal carPrice = 0;
                int year = 0;
                int mileage = 0;

            
                if (!ValidateFields(carModel, carType, transmissionType, carDesc, carImage,
                                    ref carPrice, ref year, ref mileage, driveType, color, bodyType))
                {
                    return;
                }

                using (var context = new CarShopContext())
                {
                    var newCar = new Car
                    {
                        CarModel = carModel,
                        CarType = carType,
                        TransmissionType = transmissionType,
                        CarDesc = carDesc,
                        CarPrice = carPrice,
                        CarImage = carImage,
                        Year = year,
                        DriveType = driveType,
                        Color = color,
                        Mileage = mileage,
                        BodyType = bodyType
                    };

                    context.Cars.Add(newCar);
                    context.SaveChanges();

                    var newConfig = new CarConfiguration
                    {
                        CarID = newCar.CarID,
                        Salon = txtSalon.Text.Trim(),
                        SafetySystems = txtSafetySystems.Text.Trim(),
                        Airbags = txtAirbags.Text.Trim(),
                        AssistanceSystems = txtAssistanceSystems.Text.Trim(),
                        Exterior = txtExterior.Text.Trim(),
                        Multimedia = txtMultimedia.Text.Trim()
                    };

                    context.Configuration.Add(newConfig);
                    context.SaveChanges();
                }

                ClearFields();
                MessageBox.Show("Автомобиль и комплектация успешно добавлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateFields(string carModel, string carType, string transmissionType, string carDesc,
                             string carImage, ref decimal carPrice, ref int year, ref int mileage,
                             string driveType, string color, string bodyType)
        {
           
            if (string.IsNullOrWhiteSpace(carModel) || string.IsNullOrWhiteSpace(carType) ||
                string.IsNullOrWhiteSpace(transmissionType) || string.IsNullOrWhiteSpace(carDesc) ||
                string.IsNullOrWhiteSpace(carImage) || string.IsNullOrWhiteSpace(driveType) ||
                string.IsNullOrWhiteSpace(color) || string.IsNullOrWhiteSpace(bodyType))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

         
            if (!File.Exists(carImage))
            {
                MessageBox.Show("Файл изображения не найден. Пожалуйста, выберите корректный файл.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
           
            if (!decimal.TryParse(txtCarPrice.Text.Trim(), out carPrice) || carPrice <= 0 || carPrice > 2000000)
            {
                MessageBox.Show("Введите корректную цену в диапазоне от 1 до 2 000 000.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            
            if (!int.TryParse(txtMileage.Text.Trim(), out mileage) || mileage < 0 || mileage > 2000000)
            {
                MessageBox.Show("Пробег должен быть положительным числом и не превышать 2 000 000.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(txtYear.Text.Trim(), out year) || year < 1900 || year > DateTime.Now.Year)
            {
                MessageBox.Show($"Год должен быть в диапазоне от 1900 до {DateTime.Now.Year}.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (carDesc.Length > 500)
            {
                MessageBox.Show("Описание автомобиля не должно превышать 500 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!Regex.IsMatch(color, "^[А-Яа-яA-Za-z]+$"))
            {
                MessageBox.Show("Поле 'Цвет' должно содержать только буквы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            txtCarModel.Clear();
            cmbCarType.SelectedIndex = -1;
            cmbTransmissionType.SelectedIndex = -1;
            txtCarDesc.Clear();
            txtCarPrice.Clear();
            txtCarImage.Clear();
            txtYear.Clear();
            cmbDriveType.SelectedIndex = -1;
            txtColor.Clear();
            txtMileage.Clear();
            cmbBodyType.SelectedIndex = -1;
            txtSalon.Clear();
            txtSafetySystems.Clear();
            txtAirbags.Clear();
            txtAssistanceSystems.Clear();
            txtExterior.Clear();
            txtMultimedia.Clear();
        }
    }
}

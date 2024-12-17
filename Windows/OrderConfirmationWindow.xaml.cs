using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore; 
using CarShop.Model;
namespace CarShopApp
{
    public partial class OrderConfirmationWindow : Window
    {
        private string uploadedFilePath; 
        private readonly CarShopContext _dbContext; 
        private readonly User _currentUser;

        public bool IsConfirmed { get; private set; } = false;

        public OrderConfirmationWindow(User currentUser)
        {
            InitializeComponent();
            _dbContext = new CarShopContext(); 
            _currentUser = currentUser;      

           
            if (!string.IsNullOrEmpty(_currentUser.DocumentPhoto))
            {
                uploadedFilePath = _currentUser.DocumentPhoto;
                FileNameTextBlock.Text = $"Файл уже загружен: {uploadedFilePath}";
            }
            else
            {
                FileNameTextBlock.Text = "Файл не выбран";
            }

            
            UpdateConfirmButtonState();
        }

        
        private void UploadDocument_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите документ",
                Filter = "Документы (*.jpg;*.jpeg;*.png;*.pdf)|*.jpg;*.jpeg;*.png;*.pdf"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                uploadedFilePath = openFileDialog.FileName;
                FileNameTextBlock.Text = $"Файл: {uploadedFilePath}";
            }
            else
            {
                uploadedFilePath = null;
                FileNameTextBlock.Text = "Файл не выбран";
            }

            UpdateConfirmButtonState();
        }
        
        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            UpdateConfirmButtonState();
        }

        private void UpdateConfirmButtonState()
        {
            
            ConfirmButton.IsEnabled = AgreeCheckBox.IsChecked == true && !string.IsNullOrEmpty(uploadedFilePath);
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (string.IsNullOrEmpty(_currentUser.DocumentPhoto))
                {
                    _currentUser.DocumentPhoto = uploadedFilePath;
                    _dbContext.Users.Attach(_currentUser);
                    _dbContext.Entry(_currentUser).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }

                IsConfirmed = true;

                MessageBox.Show("Заказ подтвержден! Мы свяжемся с вами.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении документа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

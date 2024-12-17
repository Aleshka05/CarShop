using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarShop.Model;
namespace CarShopApp
{
    public partial class ReviewsPage : Page
    {
        private readonly User _currentUser;
        private List<Review> _allReviews;
        private const int ReviewsPerPage = 10; 
        private int currentPage = 1;

        public ReviewsPage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadReviews();
        }

        private void LoadReviews()
        {
            using (var context = new CarShopContext())
            {
                _allReviews = context.Reviews
                    .Select(r => new Review
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        CustomerName = r.CustomerName,
                        ReviewText = r.ReviewText,
                        Date = r.Date
                    })
                    .ToList();

                DisplayReviewsForPage(currentPage);
            }
        }

        private void DisplayReviewsForPage(int page)
        {
            var reviewsForCurrentPage = _allReviews
                .Skip((page - 1) * ReviewsPerPage)
                .Take(ReviewsPerPage)
                .ToList();

            DisplayReviews(reviewsForCurrentPage);
            UpdatePaginationButtons();
        }

        private void DisplayReviews(List<Review> reviews)
        {
            ReviewsStackPanel.Children.Clear();

            foreach (var review in reviews)
            {
                var reviewPanel = new Border
                {
                    BorderBrush = System.Windows.Media.Brushes.Gray,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(10),
                    Background = System.Windows.Media.Brushes.White,
                    Padding = new Thickness(10)
                };

                var stackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };

                var customerNameText = new TextBlock
                {
                    Text = review.CustomerName,
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(10, 10, 10, 5)
                };

                var reviewText = new TextBlock
                {
                    Text = review.ReviewText,
                    FontSize = 14,
                    Margin = new Thickness(10, 0, 10, 10)
                };

                var reviewDate = new TextBlock
                {
                    Text = review.Date.ToString("dd MMM yyyy"),
                    FontSize = 12,
                    Margin = new Thickness(10, 5, 10, 10),
                    Foreground = System.Windows.Media.Brushes.Gray
                };

                stackPanel.Children.Add(customerNameText);
                stackPanel.Children.Add(reviewText);
                stackPanel.Children.Add(reviewDate);

                if (review.UserId == _currentUser.Id || _currentUser.Role.Name == "Администратор")
                {
                    var deleteButton = new Button
                    {
                        Content = "Удалить",
                        Margin = new Thickness(10),
                        Tag = review.Id
                    };

                    deleteButton.Click += DeleteReviewButton_Click;
                    stackPanel.Children.Add(deleteButton);
                }

                reviewPanel.Child = stackPanel;
                ReviewsStackPanel.Children.Add(reviewPanel);
            }
        }

        private void UpdatePaginationButtons()
        {
            int totalReviews = _allReviews.Count;
            int totalPages = (int)Math.Ceiling((double)totalReviews / ReviewsPerPage);

            btnPrevious.IsEnabled = currentPage > 1;
            btnNext.IsEnabled = currentPage < totalPages;
            lblPageNumber.Content = $"Страница {currentPage} из {totalPages}";
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                DisplayReviewsForPage(currentPage);
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            int totalReviews = _allReviews.Count;
            int totalPages = (int)Math.Ceiling((double)totalReviews / ReviewsPerPage);

            if (currentPage < totalPages)
            {
                currentPage++;
                DisplayReviewsForPage(currentPage);
            }
        }

        private void DeleteReviewButton_Click(object sender, RoutedEventArgs e)
        {
            int reviewId = (int)((Button)sender).Tag;

            using (var context = new CarShopContext())
            {
                var review = context.Reviews.FirstOrDefault(r => r.Id == reviewId);

                if (review != null)
                {
                    if (_currentUser.Role.Name == "Администратор" || review.UserId == _currentUser.Id)
                    {
                        context.Reviews.Remove(review);
                        context.SaveChanges();

                        MessageBox.Show("Отзыв удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadReviews();
                    }
                    else
                    {
                        MessageBox.Show("У вас нет прав для удаления этого отзыва.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Отзыв не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddReviewButton_Click(object sender, RoutedEventArgs e)
        {
            string newReviewText = ReviewTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(newReviewText))
            {
                var newReview = new Review
                {
                    UserId = _currentUser.Id,
                    CustomerName = _currentUser.UserName,
                    ReviewText = newReviewText,
                    Date = DateTime.Now
                };

                using (var context = new CarShopContext())
                {
                    context.Reviews.Add(newReview);
                    context.SaveChanges();
                }

                _allReviews.Add(newReview);
                DisplayReviewsForPage(currentPage);

                ReviewTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите текст отзыва.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Assignment.ViewModels;

namespace Assignment.Views
{
    /// <summary>
    /// Interaction logic for UserListView.xaml
    /// </summary>
    public partial class UserListView : Window
    {
        public UserListView()
        {
            InitializeComponent();
            DataContext = new UserListViewModel(); // Set DataContext to the ViewModel
        }
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Search...")
            {
                SearchBox.Text = "";
                SearchBox.Foreground = Brushes.Black; // Change text color to black when user types
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Search...";
                SearchBox.Foreground = Brushes.Gray; // Change text color back to gray when it's empty
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text != "Search...")
            {
                // Bind the TextBox value to SearchQuery in the ViewModel
                var viewModel = (UserListViewModel)DataContext;
                viewModel.SearchQuery = SearchBox.Text; // Make sure your ViewModel updates with the search term
            }
        }



        // Event handlers for window actions
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the LoginView
            LoginView loginView = new LoginView();
            loginView.Show(); // Show the login view

            // Close the current view (UserListView)
            this.Close();
        }




    }
}


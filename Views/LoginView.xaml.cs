using Assignment.ViewModels;
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
using System.Security;


namespace Assignment.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
        InitializeComponent();
            DataContext = new LoginViewModel();
        }
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

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                // Get the SecureString from PasswordBox
                var securePassword = new SecureString();
                foreach (char c in passwordBox.Password)
                {
                    securePassword.AppendChar(c);
                }

                // Set the Password property in ViewModel
                var viewModel = this.DataContext as LoginViewModel;
                if (viewModel != null)
                {
                    viewModel.Password = securePassword;
                }
            }
        }
            private void Button_Click(object sender, RoutedEventArgs e)
        {
            var RegisterView = new RegisterView();
            RegisterView.Show();
            this.Close();
        }

    }
}

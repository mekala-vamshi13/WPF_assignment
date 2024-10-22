using Assignment.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.InteropServices; // Fo
using Assignment.Views;
using System.Windows;
using Assignment.Connection;
using System.Data.SqlClient;

namespace Assignment.ViewModels
{
  public  class LoginViewModel:ViewModelBase
    {
        private string _email;
        private SecureString _password;
        private string _error_message;

        // Email property with getter and setter
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        // Password property with getter and setter
        public SecureString Password
        {
               get {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _error_message;
            }
            set
            {
                _error_message = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        public LoginViewModel()
        {
            // Initialize commands with their respective methods
            LoginCommand = new RelayCommand(ExecuteLogin);
            RegisterCommand = new RelayCommand(ExecuteRegister);
             ForgotPasswordCommand = new RelayCommand(ExecuteForgotPassword);
        }

        private bool CanExecuteLogin(object parameter)
        {

            return !string.IsNullOrEmpty(Email) && Password != null && Password.Length > 0;
        }

 
            private void ExecuteLogin(object obj)
            {
            if (!CanExecuteLogin(obj))
            {
                ErrorMessage = "Please enter both email and password.";
                return;
            }
            // Create an instance of the DatabaseConnection class
            var dbConnection = new DatabaseConnection();
          
                string query = "SELECT COUNT(*) FROM Usersdetail WHERE Email = @Email AND Password = @Password";
                var parameters = new[]
                {
                    new SqlParameter("@Email", Email),
                    new SqlParameter("@Password", ConvertToUnsecureString(Password)) // Ensure this matches your stored password format
                };

                int userCount = dbConnection.ExecuteScalar(query, parameters);
                if (userCount>0)
                {
                    // User exists, redirect to UserListView
                    var userListView = new UserListView(); 
                    userListView.Show();
                    Application.Current.MainWindow.Close(); 
                }
                else
                {
                    // Invalid credentials
                    ErrorMessage = "Invalid email or password.";
                }
            }
           private void ExecuteRegister(object parameter)
            {
                // Navigate to RegisterView
                var registerView = new RegisterView();
                registerView.Show();

                
                Application.Current.Windows[0].Close();
            }

        private void ExecuteForgotPassword(object parameter)
        {
            // Navigate to RegisterView
            var ForgotPassword = new ForgotPasswordView();
            ForgotPassword.Show();

            Application.Current.Windows[0].Close();
        }


        private string ConvertToUnsecureString(SecureString secureString)
        {
            if (secureString == null)
                return null;

            IntPtr pointer = IntPtr.Zero;
            try
            {
                pointer = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(pointer);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(pointer);
            }
        }


    }
}

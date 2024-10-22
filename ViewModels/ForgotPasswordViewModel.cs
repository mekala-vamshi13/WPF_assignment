using Assignment.Connection;
using Assignment.ViewModel;
using Assignment.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Assignment.ViewModels
{
    public class ForgotPasswordViewModel : ViewModelBase
    {
        private string _email;
        private SecureString _password;
        private SecureString _confirmpassword;
        private string _error_message;

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
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public SecureString ConfirmPassword
        {
            get
            {
                return _confirmpassword;
            }
            set
            {
                _confirmpassword = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public ICommand SubmitCommand { get; }
        public ForgotPasswordViewModel()
        {
            SubmitCommand = new RelayCommand(ExecuteSubmit);
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
        private void ExecuteSubmit(object obj)
        {
            // Perform validations for each field individually with specific error messages
            string passwordString = ConvertToUnsecureString(Password);
            string confirmPasswordString = ConvertToUnsecureString(ConfirmPassword);

            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Email is required.";
                return;
            }

            // Check if email contains '@gmail.com'
            if (!Email.Contains("@gmail.com"))
            {
                ErrorMessage = "Email must be a Gmail address (e.g., @gmail.com).";
                return;
            }

            if (Password == null)
            {
                ErrorMessage = "Password is required.";
                return;
            }

            if (ConfirmPassword == null)
            {
                ErrorMessage = "Confirm Password is required.";
                return;
            }

            // Check if password length is between 6 and 16 characters
            if (Password.Length < 6 || Password.Length > 16)
            {
                ErrorMessage = "Password must be between 6 and 16 characters.";
                return;
            }

            // Check if Password and ConfirmPassword are the same
            if (passwordString != confirmPasswordString)
            {
                ErrorMessage = "Password and Confirm Password must match.";
                return;
            }

            // Create an instance of the DatabaseConnection class
            var dbConnection = new DatabaseConnection();

            // Query the database to check if the email exists
            /* string checkEmailQuery = "SELECT COUNT(*) FROM Usersdetail WHERE Email = @Email";
             var emailParameter = new [] SqlParameter("@Email", Email);
             int emailExists = dbConnection.ExecuteScalar(checkEmailQuery, emailParameter);

             if (emailExists == 0)
             {
                 // Email not found
                 ErrorMessage = "User not found with the provided email.";
                 return;
             }*/

            // If the email exists, update the password
            string updatePasswordQuery = "UPDATE Usersdetail SET Password = @Password WHERE Email = @Email";
            var updateParameters = new[]
            {
                new SqlParameter("@Password", ConvertToUnsecureString(Password)),
                new SqlParameter("@Email", Email)
            };

            int rowsAffected = dbConnection.ExecuteNonQuery(updatePasswordQuery, updateParameters);

            if (rowsAffected > 0)
            {
                // Password update successful
                var userListView = new LoginView(); // Make sure to create this view
                userListView.Show();
                Application.Current.MainWindow.Close(); // Close login window if applicable
            }

            else
            {
                // Error updating password
                ErrorMessage = "Email not found";
            }
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

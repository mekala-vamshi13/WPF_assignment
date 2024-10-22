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
   public class RegisterViewModel:ViewModelBase
    {
        private string _First_name;
        private string _last_name;
        private string _gender;
        private string _email;
        private DateTime _DateofBirth;
        private string _error_message;
        private SecureString _password;
        private SecureString _confirmpassword;

        public string FirstName
        {
            get
            {
                return _First_name;
            }
            set
            {
                _First_name = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        public string LastName
        {
            get
            {
                return _last_name;
            }
            set
            {
                _last_name = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        public string Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }
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

        private DateTime DateofBirth
        {
            get
            {
                return _DateofBirth;
            }
    set
            {
                _DateofBirth = value;
                OnPropertyChanged(nameof(DateofBirth));
}
        }
        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public SecureString Confirmpassword
        {
            get { return _confirmpassword; }
            set
            {
                _confirmpassword = value;
                OnPropertyChanged(nameof(Confirmpassword));
            }
        }

        public ICommand SubmitCommand { get; }
        public RegisterViewModel()
        {
            // Initialize commands with their respective methods
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
            string passwordString = ConvertToUnsecureString(Password);
            string confirmPasswordString = ConvertToUnsecureString(Confirmpassword);

            // Perform validations for each field individually with specific error messages
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                ErrorMessage = "First Name is required.";
                return;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                ErrorMessage = "Last Name is required.";
                return;
            }

            // Assuming the default value for gender selection is "Please select an option"
            if (string.IsNullOrWhiteSpace(Gender) || Gender == "Please select an option")
            {
                ErrorMessage = "Gender is required. Please select an option.";
                return;
            }

/*            if (DateofBirth == default(DateTime))
            {
                ErrorMessage = "Date of Birth is required.";
                return;
            }*/

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

            if (Confirmpassword == null)
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


            // If all validations pass, proceed with database insertion
            var dbConnection = new DatabaseConnection();
            string formattedDateOfBirth = DateofBirth.ToString("yyyy-MM-dd");

            string query = "INSERT INTO Usersdetail (FirstName, LastName, Gender, DOB, Email, Password) VALUES (@firstName, @lastName, @gender, @DateOfBirth, @Email, @Password)";
            var parameters = new[]
            {
        new SqlParameter("@firstName", FirstName),
        new SqlParameter("@lastName", LastName),
        new SqlParameter("@gender", Gender),
        new SqlParameter("@DateOfBirth", formattedDateOfBirth),
        new SqlParameter("@Email", Email),
        new SqlParameter("@Password", ConvertToUnsecureString(Password)),
    };

            int rowsAffected = dbConnection.ExecuteNonQuery(query, parameters);
            if (rowsAffected > 0)
            {
                // Insert successful, redirect to UserListView
                var userListView = new UserListView(); // Make sure to create this view
                userListView.Show();
                Application.Current.MainWindow.Close(); // Close login window if applicable
            }
            else
            {
                // Error inserting user
                ErrorMessage = "Error inserting user.";
            }
        }


        /* private void ExecuteSubmit(object obj)
         {
             // Create an instance of the DatabaseConnection class
             var dbConnection = new DatabaseConnection();
             string formattedDateOfBirth = DateofBirth.ToString("yyyy-MM-dd");

             string query = "INSERT INTO Usersdetail (FirstName, LastName, Gender,DOB, Email, Password) VALUES (@firstName, @lastName, @gender, @DateOfBirth, @Email, @Password)";
             var parameters = new[]
             {
             new SqlParameter("@firstName", FirstName),
             new SqlParameter("@lastName", LastName),
             new SqlParameter("@gender", Gender)     ,       
             new SqlParameter("@DateOfBirth", formattedDateOfBirth),
             new SqlParameter("@Email", Email),
             new SqlParameter("@Password", ConvertToUnsecureString(Password)),
             };

             int rowsAffected = dbConnection.ExecuteNonQuery(query, parameters);
             if (rowsAffected > 0)
             {
                 // Insert successful, redirect to UserListView
                 var userListView = new UserListView(); // Make sure to create this view
                 userListView.Show();
                 Application.Current.MainWindow.Close(); // Close login window if applicable
             }
             else
             {
                 // Error inserting user
                 ErrorMessage = "Error inserting user.";
             }
         }
 */


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
 
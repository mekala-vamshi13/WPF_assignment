﻿using Assignment.ViewModels;
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

namespace Assignment.Views
{
    /// <summary>
    /// Interaction logic for ForgotPasswordView.xaml
    /// </summary>
    public partial class ForgotPasswordView : Window
    {
        public ForgotPasswordView()
        {
            InitializeComponent();
            DataContext = new ForgotPasswordViewModel();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ForgotPasswordViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).SecurePassword;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ForgotPasswordViewModel viewModel)
            {
                viewModel.ConfirmPassword = ((PasswordBox)sender).SecurePassword;
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

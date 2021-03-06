﻿using System;
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

namespace ABServer
{
    /// <summary>
    /// Логика взаимодействия для PasswordForm.xaml
    /// </summary>
    public partial class PasswordForm : Window
    {
        Clients client;

        public PasswordForm(Clients client)
        {
            InitializeComponent();
            this.client = client;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string password = passwordInput.Password.Trim();

            if (password.Length == 0)
            {
                ValidationSummary.Content = "Значение пароля не может быть пустым";
            }
            else        
            {
                if (password.Equals(passwordRepeat.Password))
                {
                    using (var db = new ClientsDBEntities())
                    {
                        client.Пароль = password;
                        db.Clients.Add(client);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Клиент успешно создан.", "Готово", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
                    this.Close();
                }
                else
                {
                    ValidationSummary.Content = "Пароли не совпадают, повторите ввод.";
                    passwordInput.Clear();
                    passwordRepeat.Clear();
                }
            }
        }

        private void passwordInput_GotFocus(object sender, RoutedEventArgs e)
        {
           ValidationSummary.Content = "";
        }
    }
}

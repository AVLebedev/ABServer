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
                //MessageBox.Show("Значение пароля не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.Cancel);
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
                        //db.Entry(client).State = System.Data.EntityState.Added;
                        //db.ChangeTracker.DetectChanges();
                        db.SaveChanges();
                    }
                    MessageBox.Show("Клиент успешно создан.", "Готово", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
                    this.Close();
                }
                else
                {
                    //MessageBox.Show("Пароли не совпадают, повторите ввод.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.Cancel);
                    ValidationSummary.Content = "Пароли не совпадают, повторите ввод.";
                    passwordInput.Clear();
                    passwordRepeat.Clear();
                }
            }
        }

        private void passwordInput_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void passwordInput_GotFocus(object sender, RoutedEventArgs e)
        {
           ValidationSummary.Content = "";
        }
    }
}

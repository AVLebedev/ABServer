using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm : Window
    {
        private Clients newClient;

        public RegistrationForm()
        {
            InitializeComponent();
            newClient = (Clients)formGrid.DataContext;
            using (ClientsDBEntities db = new ClientsDBEntities())
            {
                int lastId = db.Clients.Count() > 0 ? db.Clients.OrderByDescending(c => c.ClientId).First().ClientId : 1;
                newClient.ClientId = lastId + 1;
            }
            newClient.Пароль = "";
            newClient.Активен = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (newClient != null)
                {
                    PasswordForm passwordForm = new PasswordForm(newClient);
                    passwordForm.Show();
                    this.Hide();
                }
                else
                {
                    ValidationSummary.Content = "Исправьте ошибки";
                }
            }
            catch {
                ValidationSummary.Content = "Исправьте ошибки";
            }             
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Id.Content = newClient.ClientId;        
        }

        private void Element_GetFocus(object sender, RoutedEventArgs e)
        {
            ValidationSummary.Content = "";
        }
    }
}

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
            using (ClientsDBEntities db = new ClientsDBEntities())
            {
           // newClient = new Clients();
                newClient = new Clients();
                /*House.DataContext = newClient;
                Housing.DataContext = newClient;
                Street.DataContext = newClient;
                Office.DataContext = newClient;
                PhoneNumber.DataContext = newClient;
                Building.DataContext = newClient;
                ClientName.DataContext = newClient;
                EndDate.DataContext = newClient;
                Shedule.DataContext = newClient;*/
                int lastId = db.Clients.Count() > 0 ? db.Clients.OrderByDescending(c => c.ClientId).First().ClientId : 1;
                newClient.ClientId = lastId + 1;
            }
            //this.newClient.Пароль = "";
            //this.newClient.Активен = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (newClient != null)
                {
                    newClient.НазваниеКлиента = ClientName.Text;
                    newClient.Улица = Street.Text;
                    newClient.Дом = Convert.ToInt32(House.Text);
                    PasswordForm passwordForm = new PasswordForm(newClient);
                    passwordForm.Show();
                    this.Hide();
                }
                else
                {
                    ValidationSummary.Content = "Исправьте ошибки";
                }
               /* Clients newClient = new Clients()
                {
                    ClientId = Convert.ToInt32(Id.Content),
                    НазваниеКлиента = ClientName.Text,
                    Улица = Street.Text,
                    Дом = Convert.ToInt32(House.Text),
                    Корпус = Convert.ToInt32(Housing.Text),
                    Строение = Convert.ToInt32(Building.Text),
                    Офис = Convert.ToInt32(Office.Text),
                    Телефон = PhoneNumber.Text,
                    ДоговорИстекает = Convert.ToDateTime(EndDate.Text)
                };
                PasswordForm passwordForm = new PasswordForm(newClient); 
                passwordForm.Show(); */
                //ValidationSummary.Content = newClient.Error;
                //this.Close(); 

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

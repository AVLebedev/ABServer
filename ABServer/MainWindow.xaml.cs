using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CSVSTOViewWordInWPF;

namespace ABServer
{   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Logger.mainWindow = this;
        }

        private void clientsDBButton_Click(object sender, RoutedEventArgs e)
        {
            ClientsViewForm viewForm = new ClientsViewForm();
            viewForm.Show();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            //mainServer.StopConnection();
            //this.Close();
            Application.Current.Shutdown();
        }

        private void newClientButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationForm regForm = new RegistrationForm();
            regForm.Show();
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            bool statusOK = StartServer();
            if (statusOK)
            {
                Logger.Log("Сервер успешно запущен!");
                RequestWorker.mainWindow = this;
            }
            else
            {
                Logger.Log("Не удалось запустить сервер!");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadClients();
            //Запуск подключения
            InitializeConnection();
        }

        /// <summary>
        /// Проверка договоров клиентов по дате и вывод действующих клиентов
        /// </summary>
        public void LoadClients()
        {
            using (var db = new ClientsDBEntities())
            {
                var clientsList = db.Clients.Where(c => c.Активен == true);
                foreach (Clients c in clientsList)
                {
                    //if (c.ValidateContract())
                    //{
                    idsListBox.Items.Add(c.ClientId);
                    namesListBox.Items.Add(c.НазваниеКлиента);
                    stateListBox.Items.Add("");
                    //}
                }
            }
        }  


#region Сетевые и вспомогательные методы
        private ChatServer mainServer;

        bool StartServer()
        {
            try
            {
                mainServer = new ChatServer();
                mainServer.StartListening();
                return true;
            }
            catch(Exception e)
            {
                Logger.Log(e.Message);
                return false;
            }
        }

        private void optionsBtn_Click(object sender, RoutedEventArgs e)
        {
            Settings settingsWindow = new Settings(mainServer);
            settingsWindow.Show();
        }

        private void contractsButton_Click(object sender, RoutedEventArgs e)
        {
            var clientConracts = new CSVSTOViewWordInWPF.MainWindow();
            clientConracts.Show();
        }

#endregion

    }
}

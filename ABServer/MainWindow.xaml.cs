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
using ABServer.Controls;

namespace ABServer
{   
    public partial class MainWindow : Window
    {
        private ChatServer mainServer;
        public MainWindow()
        {
            InitializeComponent();
            Logger.mainWindow = this;
        }        

        private void InitializeConnection()
        {
            mainServer = new ChatServer();
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
            //Запуск подключения
            InitializeConnection();
            LoadClients();
        }

        /// <summary>
        /// Проверка договоров клиентов по дате и вывод действующих клиентов
        /// </summary>
        public void LoadClients()
        {
            using (var db = new ClientsDBEntities())
            {
                idsListBox.Items.Clear();
                namesListBox.Items.Clear();
                stateListBox.Items.Clear();
                var clientsList = db.Clients.Where(c => c.Активен == true).ToList();
                foreach (Clients c in clientsList)
                {
                    idsListBox.Items.Add(c.ClientId);
                    namesListBox.Items.Add(c.НазваниеКлиента);
                    stateListBox.Items.Add(new StateLabel(StateLabel.States.Disconnected, c.ClientId, mainServer));
                }
            }
        }  
        

        bool StartServer()
        {
            try
            {
                mainServer.StartListening();
                return true;
            }
            catch(Exception e)
            {
                Logger.Log(e.Message);
                return false;
            }
        }

    #region Обработчики кнопок в меню

        private void optionsBtn_Click(object sender, RoutedEventArgs e)
        {
            Settings settingsWindow = new Settings(mainServer);
            settingsWindow.Show();
        }
        private void clientsDBButton_Click(object sender, RoutedEventArgs e)
        {
            ClientsViewForm viewForm = new ClientsViewForm(this);
            viewForm.Show();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
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

        private void contractsButton_Click(object sender, RoutedEventArgs e)
        {
            var clientConracts = new CSVSTOViewWordInWPF.MainWindow();
            clientConracts.Show();
        }

    #endregion
    }
}

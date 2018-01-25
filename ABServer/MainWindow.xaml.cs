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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ABServer
{    
    public partial class MainWindow : Window
    {
        //System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        const string serverIp = "192.168.56.1";

        public MainWindow()
        {
            InitializeComponent();
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
                logLabel.Text = "Сервер успешно запущен!";
                RequestWorker.mainWindow = this;
            }
            else
            {
                logLabel.Text = "Не удалось запустить сервер!";
            }
        }

        private void ipTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            logLabel.Text = "";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Проверка договоров клиентов по дате и вывод действующих клиентов
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
            //Запуск подключения
            InitializeConnection();
        }  


#region Сетевые и вспомогательные методы
        
        const int port = 1234;
        ChatServer mainServer;

        bool StartServer()
        {
            try
            {
                IPAddress ipAddr = IPAddress.Parse(Properties.Resources.ip);
                mainServer = new ChatServer(ipAddr);
                mainServer.StartListening();
                return true;
            }
            catch(Exception e)
            {
                logLabel.Text = e.Message;
                return false;
            }
        }

        private void optionsBtn_Click(object sender, RoutedEventArgs e)
        {
        }
/*
        /// <summary>
        /// Получает сигнал от клиента, возвращает id клиента
        /// </summary>
        /// <returns></returns>
        private int GetSignalFromClient()
        {
            StreamReader reader = new StreamReader();
            try
            {
                int id = Convert.ToInt32(reader.ReadLine());
                return id;
            }
            catch
            {
                return -1;
            }
        } */
      
      /*  private string read()
        {
            try
            {
                string[] rtext = reader.ReadLine().Split(' ');
                text_data.Text += "Переданные числа:" + Environment.NewLine;
                foreach (string n in rtext)
                {
                    text_data.Text += n + ' ';
                }
                text_data.Text += Environment.NewLine;

                return rtext.ToString();
            }
            catch
            {
                return "";
            }
        }*/

#endregion

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections;

namespace ABServer
{
    public class StatusChangedEventArgs : EventArgs
    {
        private string EventMsg;

        public string EventMessage
        {
            get
            {
                return EventMsg;
            }
            set
            {
                EventMsg = value;
            }
        }

        public StatusChangedEventArgs(string strEventMsg)
        {
            EventMsg = strEventMsg;
        }
    }

    public delegate void StatusChangedEventHandler(object sender, StatusChangedEventArgs e);

    public class ChatServer
    {
        
        public static Hashtable htUsers = new Hashtable(); 
        
        public static Hashtable htConnections = new Hashtable(); 
        private IPAddress ipAddress;
        private TcpClient tcpClient;
        
        public static event StatusChangedEventHandler StatusChanged;
        private static StatusChangedEventArgs e;

        public ChatServer()
        {
            // Получение локального IPv4
            string strHostName = System.Net.Dns.GetHostName(); ;
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            int i = 0;
            ipAddress = addr[i];
            while (addr[i].AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                i++;
                ipAddress = addr[i];
            }
        }

        private Thread thrListener;
        private TcpListener tlsClient;

        bool ServRunning = false;

        public void StartListening()
        {            
            int port = Properties.Settings.Default.Port;
            // Создание подключения
            tlsClient = new TcpListener(ipAddress, port);

            // Старт
            tlsClient.Start();

            
            ServRunning = true;

            thrListener = new Thread(KeepListening);
            thrListener.Start();
        }

        public void StopListening()
        {
            ServRunning = false;
        }

        private Connection newConnection;
        private void KeepListening()
        {
            while (ServRunning == true)
            {
                tcpClient = tlsClient.AcceptTcpClient();
                newConnection = new Connection(tcpClient);
            }
        }

        /// <summary>
        /// Отправка ответа клиенту, что сигнал тревоги принят и обработан
        /// </summary>
        public void AlarmResponse()
        {
            newConnection.AlarmResponse();
        }
    }

    class Connection
    {
        TcpClient tcpClient;
        private Thread thrSender;
        private StreamReader srReceiver;
        private StreamWriter swSender;
        private string strResponse;
        private RequestWorker worker;

        public Connection(TcpClient tcpCon)
        {
            tcpClient = tcpCon;
            worker = new RequestWorker(tcpClient);
            thrSender = new Thread(AcceptClientRequest);
            thrSender.Start();
        }

        private void CloseConnection()
        {
            tcpClient.Close();
            srReceiver.Close();
            swSender.Close();
        }

        private void AcceptClientRequest()
        {
            srReceiver = new System.IO.StreamReader(tcpClient.GetStream());
            worker.AnalizeRequest(srReceiver.ReadLine());
            
            while ((strResponse = srReceiver.ReadLine()) != "")
            {
                if (strResponse == null)
                { 
                    return;
                }
                else
                {
                    worker.AnalizeRequest(strResponse);                        
                }
            }
        }
        
        /// <summary>
        /// Отправка ответа клиенту, что сигнал тревоги принят и обработан
        /// </summary>
        public void AlarmResponse() {
            worker.ResponseOk();
        }
    }
}

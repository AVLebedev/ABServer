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

    class ChatServer
    {
        
        public static Hashtable htUsers = new Hashtable(); 
        
        public static Hashtable htConnections = new Hashtable(); 
        private IPAddress ipAddress;
        private TcpClient tcpClient;
        
        public static event StatusChangedEventHandler StatusChanged;
        private static StatusChangedEventArgs e;

        public ChatServer(IPAddress address)
        {
            ipAddress = address;
        }

        private Thread thrListener;
        private TcpListener tlsClient;

        bool ServRunning = false;

      /*  public static void AddUser(TcpClient tcpUser, string strUsername)
        {
            //ChatServer.htUsers.Add(strUsername, tcpUser);
            ChatServer.htConnections.Add(tcpUser, strUsername);


            //SendAdminMessage(htConnections[tcpUser] + " присоедининился");
        } */

     /*   public static void RemoveUser(TcpClient tcpUser)
        {
            if (htConnections[tcpUser] != null)
            {
                //SendAdminMessage(htConnections[tcpUser] + " отключился");

                ChatServer.htUsers.Remove(ChatServer.htConnections[tcpUser]);
                ChatServer.htConnections.Remove(tcpUser);
            }
        } */

     /*   public static void OnStatusChanged(StatusChangedEventArgs e)
        {
            StatusChangedEventHandler statusHandler = StatusChanged;
            if (statusHandler != null)
            {
                statusHandler(null, e);
            }
        } */

       /* 
        public static void SendAdminMessage(string Message)
        {
            StreamWriter swSenderSender;

            
            e = new StatusChangedEventArgs("Сервер: " + Message);
            OnStatusChanged(e);

            TcpClient[] tcpClients = new TcpClient[ChatServer.htUsers.Count];
            ChatServer.htUsers.Values.CopyTo(tcpClients, 0);
            for (int i = 0; i < tcpClients.Length; i++)
            {
                try
                {
                    if (Message.Trim() == "" || tcpClients[i] == null)
                    {
                        continue;
                    }
                    swSenderSender = new StreamWriter(tcpClients[i].GetStream());
                    swSenderSender.WriteLine("Сервер: " + Message);
                    swSenderSender.Flush();
                    swSenderSender = null;
                }
                catch 
                {
                    RemoveUser(tcpClients[i]);
                }
            }
        } */

     /*   public static void SendMessage(string From, string Message)
        {
            StreamWriter swSenderSender;

            e = new StatusChangedEventArgs(From + " : " + Message);
            OnStatusChanged(e);

            TcpClient[] tcpClients = new TcpClient[ChatServer.htUsers.Count];
            ChatServer.htUsers.Values.CopyTo(tcpClients, 0);
            for (int i = 0; i < tcpClients.Length; i++)
            {
                try
                {
                    if (Message.Trim() == "" || tcpClients[i] == null)
                    {
                        continue;
                    }
                    swSenderSender = new StreamWriter(tcpClients[i].GetStream());
                    swSenderSender.WriteLine(From + " : " + Message);
                    swSenderSender.Flush();
                    swSenderSender = null;
                }
                catch 
                {
                    RemoveUser(tcpClients[i]);
                }
            }
        } */

        public void StartListening()
        {

            IPAddress ipaLocal = ipAddress;

            // Создание подключения
            tlsClient = new TcpListener(ipaLocal, Properties.Resources.port);

            // Старт
            tlsClient.Start();

            
            ServRunning = true;

            thrListener = new Thread(KeepListening);
            thrListener.Start();
        }
        
        private void KeepListening()
        {
            while (ServRunning == true)
            {
                tcpClient = tlsClient.AcceptTcpClient();
                Connection newConnection = new Connection(tcpClient);
            }
        }
    }

    class Connection
    {
        TcpClient tcpClient;
        private Thread thrSender;
        private StreamReader srReceiver;
        private StreamWriter swSender;
        private string currUser;
        private string strResponse;

        public Connection(TcpClient tcpCon)
        {
            tcpClient = tcpCon;
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
            RequestWorker worker = new RequestWorker(tcpClient);
            srReceiver = new System.IO.StreamReader(tcpClient.GetStream());
            //swSender = new System.IO.StreamWriter(tcpClient.GetStream());
            //worker.clientTcp = tcpClient;
            worker.AnalizeRequest(srReceiver.ReadLine());

            /*currUser = srReceiver.ReadLine()[0];

            
                if (ChatServer.htUsers.Contains(currUser) == true)
                {
                    swSender.WriteLine("0|Этот клиент уже подключён.");
                    swSender.Flush();
                    CloseConnection();
                    return;
                }
                else
                {
                    swSender.WriteLine("1");
                    swSender.Flush();

                    //ChatServer.AddUser(tcpClient, currUser);
                } */ 

            try
            {
                while ((strResponse = srReceiver.ReadLine()) != "")
                {
                    if (strResponse == null)
                    {
                        //ChatServer.RemoveUser(tcpClient);  
                        return;
                    }
                    else
                    {
                        //ChatServer.SendMessage(currUser, strResponse);
                        worker.AnalizeRequest(strResponse);                        
                    }
                }
            }
            catch
            {
                //ChatServer.RemoveUser(tcpClient);
                return;
            }
        }

        
    }
}

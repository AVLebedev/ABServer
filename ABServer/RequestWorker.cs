using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ABServer
{
    public class RequestWorker
    {
        private static MainWindow workForm;
        public static MainWindow mainWindow 
        { 
            set
            {
                workForm = value;
            }
        }

        /*private TcpClient tcpClient;
        public  TcpClient clientTcp
        {
            set
            {
                this.tcpClient = value;
            }
        }*/

        private StreamReader srReceiver;
        private StreamWriter swSender;
        public RequestWorker(TcpClient tcpClient)
        {
            srReceiver = new StreamReader(tcpClient.GetStream());
            swSender = new StreamWriter(tcpClient.GetStream());
        }

        /// <summary>
        /// Выполняет анализ полученного от клиента сообщения и производит необходимые действия
        /// </summary>
        /// <param name="message"></param>
        public void AnalizeRequest(string message)
        {
            string[] msgParts = message.Split(new char[] { '|' });
            int id = Convert.ToInt32(msgParts[0]);
            string command = msgParts[1];
            switch (command)
            {
                case "activate":
                    //workForm.Dispatcher.Invoke(new Action(() => { ActivateClient(id); }));
                    ActivateClient(id, msgParts[2]);
                    break;
                case "connect":
                    workForm.Dispatcher.Invoke(new Action(() => { EnableClient(id); }));
                    break;
                case "alarm":
                    workForm.Dispatcher.Invoke(new Action(() => { Alarm(id); }));
                    break;
                case "disconnect":
                    workForm.Dispatcher.Invoke(new Action(() => { DisableClient(id); }));
                    break;
            }
        }

        /// <summary>
        /// Производит активацию клиента и отправляет ему сообщение о результате активации
        /// </summary>
        /// <param name="activationData">данные для активации</param>
        private void ActivateClient(int id, string code)
        {
            using (var db = new ClientsDBEntities())
            {
                Clients client = db.Clients.FirstOrDefault(c => c.ClientId == id);
                if (client != null && client.Пароль == code)
                {
                    //client.ValidateContract();
                    if (client.Активен == false)
                    {
                        client.Активен = true;
                        db.SaveChanges();
                        workForm.idsListBox.Items.Add(id);
                        workForm.namesListBox.Items.Add(client.НазваниеКлиента);
                        Logger.Log(client.НазваниеКлиента + " (id = " + id + ") успешно активирован.");
                    }
                    swSender.WriteLine("1");
                    swSender.Flush();
                }
                else
                {
                    swSender.WriteLine("0");
                    swSender.Flush();
                }
            }
        }

        private static void DisableClient(int id)
        {
            try
            {
                int index = workForm.idsListBox.Items.IndexOf(id);
                workForm.stateListBox.Items[index] = "N";
                string clientName;
                using (var db = new ClientsDBEntities())
                {
                    clientName = db.Clients.FirstOrDefault(c => c.ClientId == id).НазваниеКлиента;
                }
                Logger.Log(clientName + " (id = " + id + ") отключился.");
            }
            catch
            {
            }
        }

        private static void Alarm(int id)
        {
            int index = workForm.idsListBox.Items.IndexOf(id);
            workForm.stateListBox.Items[index] = "A!";
            string clientName;
            using (var db = new ClientsDBEntities())
            {
                clientName = db.Clients.FirstOrDefault(c => c.ClientId == id).НазваниеКлиента;
            }
            Logger.Log(clientName + " (id = " + id + ") - тревога!");
        }

        private static void EnableClient(int id)
        {
            try
            {
                int index = workForm.idsListBox.Items.IndexOf(id);
                workForm.stateListBox.Items[index] = "Y";
                string clientName;
                using (var db = new ClientsDBEntities())
                {
                    clientName = db.Clients.FirstOrDefault(c => c.ClientId == id).НазваниеКлиента;
                }
                Logger.Log(clientName + " (id = " + id + ") подключился.");
            }
            catch { }
        }
    }
}

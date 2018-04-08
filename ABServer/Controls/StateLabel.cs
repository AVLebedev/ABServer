using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;

namespace ABServer.Controls
{
    class StateLabel : Label
    {
        public enum States { Connected, Disconnected, Alarm };

        private int clientId;
        private ChatServer chatServer;
        public StateLabel(States initState, int clientId, ChatServer chatServer)
        {
            this.clientId = clientId;
            this.chatServer = chatServer;
            BorderThickness = new System.Windows.Thickness(5);
            switchState(initState);
        }

        public void switchState(States state)
        {
            switch (state)
            {
                case States.Connected:
                    this.BorderBrush = null; //= Brushes.Transparent;
                    this.Background = Brushes.Green;  
                    break;
                case States.Disconnected:
                    this.BorderBrush = Brushes.Transparent;
                    this.Background = Brushes.Red;
                    break;
                case States.Alarm:
                    this.MouseDown += alarmHandler;
                    this.BorderBrush = Brushes.Red;
                    this.Background = Brushes.Yellow;
                    break;
            }
        }

        private void alarmHandler(object sender, RoutedEventArgs e)
        {
            this.MouseDown -= alarmHandler;
            switchState(States.Connected);
            using (var db = new ClientsDBEntities())
            {
                Clients client = db.Clients.FirstOrDefault(c => c.ClientId == clientId);
                client.Вызовов += 1;
                db.SaveChanges();
            }
            chatServer.AlarmResponse();
        }
    }
}

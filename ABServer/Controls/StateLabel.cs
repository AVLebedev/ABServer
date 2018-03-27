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
        public StateLabel(States initState, int clientId)
        {
            this.clientId = clientId;
            BorderThickness = new System.Windows.Thickness(5);
            switchState(initState);
        }

        public void switchState(States state)
        {
            switch (state)
            {
                case States.Connected:
                    this.BorderBrush = null;
                    this.Background = Brushes.Green;  
                    break;
                case States.Disconnected:
                    this.BorderBrush = null;
                    this.Background = Brushes.Red;
                    break;
                case States.Alarm:
                    this.BorderBrush = Brushes.Red;
                    this.Background = Brushes.Yellow;
                    break;
            }
        }

        private void alarmHandler(object sender, RoutedEventArgs e)
        {
            switchState(States.Connected);
            using (var db = new ClientsDBEntities())
            {
                Clients client = db.Clients.FirstOrDefault(c => c.ClientId == clientId);
                client.Вызовов += 1;
                db.SaveChanges();
            }
        }
    }
}

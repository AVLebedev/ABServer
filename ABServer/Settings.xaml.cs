using System;
using System.Windows;

namespace ABServer
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private ChatServer currentServer;
        public Settings(ChatServer server)
        {
            InitializeComponent();
            currentServer = server;
            portInput.Text = Properties.Settings.Default.Port.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Port = Convert.ToInt32(portInput.Text);
            Properties.Settings.Default.Save();
            MessageBox.Show("Настройки успешно сохранены. Чтобы применить изменения, перезапустите приложение.");
            Close();
        }
    }
}

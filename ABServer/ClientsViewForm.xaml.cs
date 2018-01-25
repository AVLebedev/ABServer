using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для ClientsViewForm.xaml
    /// </summary>
    public partial class ClientsViewForm : Window
    {
        DbSet<Clients> dbSet;
       // IEnumerable<Clients> clients;
        public ClientsViewForm()
        {
            InitializeComponent();
            using (ClientsDBEntities db = new ClientsDBEntities())
            {
               // clients = db.Clients.Select(c => c).ToList<Clients>();
                dbSet = db.Set<Clients>();
                dbSet.Load();
                dataGrid.ItemsSource = dbSet.Local.ToBindingList();
            } 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {        
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            using (ClientsDBEntities db = new ClientsDBEntities())
            {
                db.ChangeTracker.DetectChanges();
                db.SaveChanges();
            }
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            using (ClientsDBEntities db = new ClientsDBEntities())
            {
                db.SaveChanges();
            }
        }
    }
}

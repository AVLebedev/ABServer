using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        private ClientsDBEntities db = new ClientsDBEntities();
        private List<Clients> clients;
        private bool hasChangesCommitted = true;
        private MainWindow mainWindow;

        public ClientsViewForm(MainWindow mainWindow)
        {
            InitializeComponent();
            clients = db.Clients.Select(c => c).ToList();
            dataGrid.ItemsSource = clients;
            this.mainWindow = mainWindow;
        }

        private void DetectChanges()
        {
            hasChangesCommitted = false;
            this.Title += " *";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {        
        }

        /// <summary>
        /// Обработчик редактирования ячейки DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int clientId = (e.Row.Item as Clients).ClientId; //или (e.EditingElement.DataContext as Clients).ClientId
            Clients client = getClientById(clientId);
            object newValue = getEditedValue(e.EditingElement);

            PropertyInfo propertyInfo = client.GetType().GetProperty(e.Column.Header.ToString());
            //try
            //{
                propertyInfo.SetValue(client, Convert.ChangeType(newValue, propertyInfo.PropertyType), null);
            //}
            //catch (InvalidCastException exc)
            //{
            //    newValue = castValueFormat(newValue);
            //}

            DetectChanges();
            //db.SaveChanges();
        }

        /// <summary>
        /// Получает значение из элемента ячейки DataGrid
        /// </summary>
        /// <param name="cellElement">элемент ячейки</param>
        /// <returns>значение элемента (тип неизвестен)</returns>
        private object getEditedValue(object cellElement)
        {
            object value = null;
            Type elementType = cellElement.GetType();

            if (elementType == typeof(TextBox))
                value = (cellElement as TextBox).Text;
            else if (elementType == typeof(CheckBox))
                value = (cellElement as CheckBox).IsChecked;
            
            return value;
        }

        //private object castValueFormat(out object value)
        //{
        //    if (value.GetType() == typeof(int))
        //    {
        //        value = Convert.
        //    }
        //}

        /// <summary>
        /// Получает клиента из списка клиентов
        /// </summary>
        /// <param name="id">значение id клиента</param>
        /// <returns>объект клиента</returns>
        private Clients getClientById(int id)
        {
            return clients.SingleOrDefault(c => c.ClientId == id);
        }

        /// <summary>
        /// Обработчик нажатия кнопки сохранения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {            
            this.Title = this.Title.TrimEnd(new char[] { ' ', '*' });
            saveChanges();
            mainWindow.Dispatcher.Invoke(new Action(() => { mainWindow.LoadClients(); }));
            MessageBox.Show("Изменения успешно сохранены", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);            
        }

        /// <summary>
        /// Сохранение изменений и обновление списка активных клиентов
        /// </summary>
        private void saveChanges()
        {
            try
            {
                db.SaveChanges();
                hasChangesCommitted = true;
                //ToDo: вызывать LoadClients() основного окна
            }
            catch
            {
                MessageBox.Show("Изменения не удалось сохранить!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        ///// <summary>
        ///// Обработчик нажатия кнопки удаления
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void deleteButton_Click(object sender, RoutedEventArgs e)
        //{
        //    int clientId = (dataGrid.SelectedItem as Clients).ClientId;
        //    Clients client = getClientById(clientId);
        //    db.Clients.Remove(client);
        //    hasChangesCommitted = false;
        //}

        /// <summary>
        /// Обработчик закрытия окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (hasChangesCommitted == false)
            {
                MessageBoxResult result = MessageBox.Show("Сохранить изменения?", "", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    saveChanges();
                }
                else if (result == MessageBoxResult.No)
                {
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

    }
}

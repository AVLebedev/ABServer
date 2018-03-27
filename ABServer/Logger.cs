using System;

namespace ABServer
{
    public static class Logger
    {
        private static MainWindow workForm;
        public static MainWindow mainWindow
        {
            set
            {
                workForm = value;
            }
        }

        public static void Log(string message)
        {
            workForm.Dispatcher.Invoke(new Action(() => { log(message); }));
        }
        private static void log(string message)
        {
            workForm.logLabel.Text += String.Format(@"{0}: {1}", DateTime.Now.ToString(), message) + "\n";
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABServer.ViewModels
{
    class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            Model = new Clients();
        }

        public Clients Model
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

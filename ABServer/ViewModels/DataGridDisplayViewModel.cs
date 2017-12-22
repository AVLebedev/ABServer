using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABServer.ViewModels
{
    public class DataGridDisplayViewModel
    {
        private BindingList<Clients> _clients;
        /*  private double _totalExpense;

        public double TotalExpense
        {
            get { return _totalExpense = _expenses.Sum(x => x.Amount); }
            set { _totalExpense = value; }
        } */
       
        public BindingList<Clients> Expenses
        {
            get 
            { 
                return _clients; 
            }
            set
            {
                _clients = value;
            }
        }


        public DataGridDisplayViewModel()
        {
        }
    }
}

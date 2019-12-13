using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FTWrapper;
using Futu.OpenApi.Pb;

namespace FutuOrderPlace
{
    public class Commands
    {
        public ICommand Unlock { get; set; } = new Unlock();
        public ICommand Activate { get; set; } = new Activate();
        public ICommand ModifyOrder { get; set; } = new ModifyOrder();
        public ICommand CancelOrder { get; set; } = new CancelOrder();
    }

    class Unlock : ICommand
    {
        public bool CanExecute(object parameter)
        {
            IController ctrl = parameter as IController;
            if (ctrl is FTController && ((FTController)ctrl).IsConnected)
                return true;
            else
                return false;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            MainViewModel vm = MainViewModel.Instance;
            IController ctrl = parameter as IController;
            ((FTController)ctrl).Lock();
        }
    }

    class Activate : ICommand
    {
        public bool CanExecute(object parameter)
        {
            MainViewModel vm = MainViewModel.Instance;
            if (parameter != null)
            {
                int id = (int)parameter;
                ScheduledOrder order = vm.OrderList.FirstOrDefault(x => x.Id == id);
                if (order != null)
                {
                    if (order.IsSent)
                        return false;
                    else
                    {
                        if (order.IsActivated)
                            return true;

                        if (!order.IsActivated)
                        {
                            if (order.ScheduledTime < DateTime.Now)
                                return false;
                            else
                                return true;
                        }                            
                    }                    
                }
            }
            return false;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            MainViewModel vm = MainViewModel.Instance;
            int id = (int)parameter;
            ScheduledOrder order = vm.OrderList.FirstOrDefault(x => x.Id == id);
            if (order != null)
                order.IsActivated = !order.IsActivated;
        }
    }

    class ModifyOrder : ICommand
    {
        public bool CanExecute(object parameter)
        {
            MainViewModel vm = MainViewModel.Instance;
            if (parameter != null)
            {
                int id = (int)parameter;
                ScheduledOrder order = vm.OrderList.FirstOrDefault(x => x.Id == id);
                if (order != null)
                    return !order.IsSent;
            }
            return false;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            MainViewModel vm = MainViewModel.Instance;
            int id = (int)parameter;
            ScheduledOrder order = vm.OrderList.FirstOrDefault(x => x.Id == id);
            if (order != null)
                order.IsActivated = !order.IsActivated;
        }
    }

    class CancelOrder : ICommand
    {
        public bool CanExecute(object parameter)
        {
            MainViewModel vm = MainViewModel.Instance;
            if (parameter != null)
            {
                int id = (int)parameter;
                ScheduledOrder order = vm.OrderList.FirstOrDefault(x => x.Id == id);
                if (order != null)
                    return !order.IsSent;
            }
            return false;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            MainViewModel vm = MainViewModel.Instance;
            int id = (int)parameter;
            ScheduledOrder order = vm.OrderList.FirstOrDefault(x => x.Id == id);
            if (order != null)
                order.IsActivated = !order.IsActivated;
        }
    }
}


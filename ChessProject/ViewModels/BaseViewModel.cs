using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ChessProject.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public bool SetProperty<T>(ref T backStore, T newValue,[CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backStore, newValue)) return false;
            backStore = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed == null) return;
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

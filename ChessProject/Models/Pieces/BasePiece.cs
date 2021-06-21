using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ChessProject.Models
{
    public abstract class BasePiece : INotifyPropertyChanged
    {
        #region INotifyPropertychanged field and methods
        public event PropertyChangedEventHandler PropertyChanged;
        public bool SetProperty<T>(ref T backStore, T newValue, [CallerMemberName] string propName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backStore, newValue)) return false;
            backStore = newValue;
            OnPropertyChanged(propName);
            return true;
        }

        private void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion

        public string Name { get; set; }
        public Positions Positions { get; set; }
        public Positions InitPositions { get; set; }
        public bool IsKnockedOut { get; set; }

    }
}

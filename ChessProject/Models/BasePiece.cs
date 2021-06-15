using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ChessProject.Models
{
    public abstract class BasePiece : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }

        

    }
}

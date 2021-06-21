using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChessProject.Models
{
    public class Side
    {
        public string Name { get; set; }

        public string SidePosition { get; set; }
        public ObservableCollection<BasePiece> Pieces {get;set;}
    }
}

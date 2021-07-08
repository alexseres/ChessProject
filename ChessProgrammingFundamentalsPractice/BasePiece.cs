using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public abstract class BasePiece
    {
        public ulong Positions { get; set; }
        public string Name { get; set; }
        public string BoardName { get; set; }
    }
}

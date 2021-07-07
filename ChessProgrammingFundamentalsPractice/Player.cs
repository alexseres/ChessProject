using System;
using System.Collections.Generic;
using System.Text;



namespace ChessProgrammingFundamentalsPractice
{
    public class Player
    {
        public ColorSide Color { get; set; }
        public ulong Pieces { get; set; }
        public ulong Rooks { get; set; }
        public ulong Knights { get; set; }
        public ulong Bishops { get; set; }
        public ulong Queen { get; set; }
        public ulong King { get; set; }
        public ulong Pawns { get; set; }
    }
}

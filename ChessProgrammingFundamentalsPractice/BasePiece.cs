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

        public ColorSide Color { get; set; }

        public BasePiece(ColorSide color)
        {
            Color = color;
        }

        public abstract void Move(int pos);

        public abstract ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions);
    }
}

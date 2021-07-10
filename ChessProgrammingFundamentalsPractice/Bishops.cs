using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class Bishops : BasePiece
    {
        public override void Move(int pos)
        {
            throw new NotImplementedException();
        }

        public Bishops(ColorSide color) : base(color)
        {

        }
        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            throw new NotImplementedException();
        }
    }
}

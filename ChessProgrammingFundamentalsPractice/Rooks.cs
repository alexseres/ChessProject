﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class Rooks : BasePiece
    {
        public Rooks(ColorSide color) : base(color)
        {

        }

        public override void Move(int pos)
        {
            throw new NotImplementedException();
        }

        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            throw new NotImplementedException();
        }
    }
}

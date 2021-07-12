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

        public BasePiece(ColorSide color, ulong positions)
        {
            Color = color;
            Positions = positions;
        }


        public void UpdatePositionWhenMove(ulong currentPosition, ulong opportunities, ulong decidedMovePos)
        {
            if ((opportunities & decidedMovePos) > 0)
            {
                Positions = (~currentPosition & Positions) | decidedMovePos;
            }
        }

        public void UpdatePositionWhenBeingAttacked(ulong attackedPosition)
        {
            if((attackedPosition & Positions) > 0)
            {
                Positions = (Positions & ~attackedPosition);
            }
        }

        public abstract ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions);
    }
}

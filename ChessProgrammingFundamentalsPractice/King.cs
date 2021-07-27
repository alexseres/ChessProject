using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class King : BasePiece
    {
        private ulong MaskNotInAFile = 0b_0111_1111_0111_0111_1111_0111_1111_1111_1111_0111_1111_0111_1111_0111_1111_0111;
        private ulong MaskNotInHFile = 0b_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110;

        public King(ColorSide color, ulong positions): base(color, positions)
        {
            
        }

        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            ulong northAttack = currentPosition << 8;
            ulong northEastAttack = (currentPosition & MaskNotInHFile) << 7;
            ulong northWestAttack = (currentPosition & MaskNotInAFile) << 9;
            ulong westAttack = (currentPosition & MaskNotInAFile) << 1;
            ulong eastAttack = (currentPosition & MaskNotInHFile) >> 1;
            ulong southEastAttack = (currentPosition & MaskNotInHFile) >> 9;
            ulong southWestAttack =(currentPosition & MaskNotInAFile) >> 7;
            ulong southAttack = currentPosition >> 8;
            ulong allAttacks = northAttack ^ northEastAttack ^ northWestAttack ^ westAttack ^ eastAttack ^ southEastAttack ^ southWestAttack ^ southAttack;
            ulong allPossibilities = (allAttacks & ~ourPositions);
            return allPossibilities;
        }

        public bool CheckIfCheck(List<IObserver> PieceListOfOpponent, ulong opponentPositions, ulong allPositionAtBoard, ulong kingPosition)
        {
            ulong mask = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            for(int i = 0;i < 64; i++)
            {
                if((opponentPositions & mask) > 0)
                {
                    foreach(IObserver observer in PieceListOfOpponent)
                    {
                        BasePiece piece = observer as BasePiece;
                        if ((piece.Positions & mask) > 0)
                        {
                            ulong attacks = piece.Search(mask, allPositionAtBoard, opponentPositions, (allPositionAtBoard & ~opponentPositions));
                            if((attacks & kingPosition) > 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                mask = mask >> 1;
            }

            return false;
        }
    }
}

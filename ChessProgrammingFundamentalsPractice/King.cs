using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class King : BasePiece
    {
        private ulong MaskNotInAFile = 0b_0111_1111_0111_0111_1111_0111_1111_1111_1111_0111_1111_0111_1111_0111_1111_0111;
        private ulong MaskNotInHFile = 0b_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110;

        private const int NorthDirection = 8;
        private const int EastDiretion = -1;
        private const int SouthDirection = -8;
        private const int WestDirection = 1;
        private const int EastNorthDirection = 7;
        private const int WestNorthDirection = 9;
        private const int EastSouthDirection = -9;
        private const int WestSouthDirection = -7;

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
    }
}

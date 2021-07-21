using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class Knights : BasePiece
    {
        private ulong MaskNotInAFile = 0b_0111_1111_0111_0111_1111_0111_1111_1111_1111_0111_1111_0111_1111_0111_1111_0111;
        private ulong MaskNotInABFile = 0b_0011_1111_0011_1111_0011_1111_0011_1111_0011_1111_0011_1111_0011_1111_0011_1111;
        private ulong MaskNotInHFile = 0b_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110;
        private ulong MaskNotInGHFile = 0b_1111_1100_1111_1100_1111_1100_1111_1100_1111_1110_1111_1100_1111_1100_1111_1100;

        public Knights(ColorSide color, ulong positions) : base(color, positions)
        {

        }


        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            ulong northNorthEast = (currentPosition & MaskNotInHFile) << 15;
            ulong northEastEast = (currentPosition & MaskNotInGHFile) << 6;
            ulong northNorthWest = (currentPosition & MaskNotInAFile) << 17;
            ulong northWestWest = (currentPosition & MaskNotInABFile) << 10;
            ulong southEastEast = (currentPosition & MaskNotInGHFile) >> 10;
            ulong southSouthEast = (currentPosition & MaskNotInGHFile) >> 17;
            ulong southSouthWest = (currentPosition & MaskNotInAFile) >> 15;
            ulong southWestWest = (currentPosition & MaskNotInABFile) >> 6;
            ulong allDirection = northNorthEast ^ northEastEast ^ northNorthWest ^ northWestWest ^ southEastEast ^ southSouthEast ^ southWestWest ^ southSouthWest;
            ulong allPossibleMove = allDirection & ~ourPositions;
            return allPossibleMove;
        }


    }
}

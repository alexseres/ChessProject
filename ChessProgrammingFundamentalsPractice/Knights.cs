using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    [Serializable]
    public class Knights : BasePiece
    {
        private ulong MaskNotInAFile = 0b_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111;
        private ulong MaskNotInABFile = 0b_0011_1111_0011_1111_0011_1111_0011_1111_0011_1111_0011_1111_0011_1111_0011_1111;
        private ulong MaskNotInHFile = 0b_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110;
        private ulong MaskNotInGHFile = 0b_1111_1100_1111_1100_1111_1100_1111_1100_1111_1110_1111_1100_1111_1100_1111_1100;

        public Knights(Player player, ColorSide color, ulong position, string boardName) : base(player, color, position, boardName)
        {
            Name = "Knight";
        }


        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            ulong northNorthEast = (currentPosition & MaskNotInHFile) << 15;
            ulong northEastEast = (currentPosition & MaskNotInGHFile) << 6;
            ulong northNorthWest = (currentPosition & MaskNotInAFile) << 17;
            ulong northWestWest = (currentPosition & MaskNotInABFile) << 10;
            ulong southEastEast = (currentPosition & MaskNotInGHFile) >> 10;
            ulong southSouthEast = (currentPosition & MaskNotInHFile) >> 17;
            ulong southSouthWest = (currentPosition & MaskNotInAFile) >> 15;
            ulong southWestWest = (currentPosition & MaskNotInABFile) >> 6;
            ulong allDirection = northNorthEast ^ northEastEast ^ northNorthWest ^ northWestWest ^ southEastEast ^ southSouthEast ^ southWestWest ^ southSouthWest;
            ulong allPossibleMove = allDirection & ~ourPositions;
            return allPossibleMove;
        }

        public override ulong GetSpecificAttackFromSearch(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions, ulong opponentPiecePosition)
        {
            ulong[] allMoves = new ulong[8];
            ulong northNorthEast = (currentPosition & MaskNotInHFile) << 15;
            ulong northEastEast = (currentPosition & MaskNotInGHFile) << 6;
            ulong northNorthWest = (currentPosition & MaskNotInAFile) << 17;
            ulong northWestWest = (currentPosition & MaskNotInABFile) << 10;
            ulong southEastEast = (currentPosition & MaskNotInGHFile) >> 10;
            ulong southSouthEast = (currentPosition & MaskNotInHFile) >> 17;
            ulong southSouthWest = (currentPosition & MaskNotInAFile) >> 15;
            ulong southWestWest = (currentPosition & MaskNotInABFile) >> 6;
            allMoves[0] = northNorthEast;
            allMoves[1] = northEastEast;
            allMoves[2] = northNorthWest;
            allMoves[3] = northWestWest;
            allMoves[4] = southEastEast;
            allMoves[5] = southSouthEast;
            allMoves[6] = southSouthWest;
            allMoves[7] = southWestWest;
            for (int i =0;i < allMoves.Length;i++)
            {
                if ((allMoves[i] & opponentPiecePosition) > 0)
                {
                    return allMoves[i];
                }
            }

            return 0;
        }
    }
}




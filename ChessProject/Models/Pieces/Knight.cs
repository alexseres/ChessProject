using ChessProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.Models.Pieces
{
    public class Knight : BasePiece
    {
        private ulong MaskNotInAFile = 0b_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111;
        private ulong MaskNotInABFile = 0b_0011_1111_0011_1111_0011_1111_0011_1111_0011_1111_0011_1111_0011_1111_0011_1111;
        private ulong MaskNotInHFile = 0b_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110;
        private ulong MaskNotInGHFile = 0b_1111_1100_1111_1100_1111_1100_1111_1100_1111_1110_1111_1100_1111_1100_1111_1100;

        public Knight(Player player, ColorSide color, ulong position, string imagePath) : base(player, color, position, imagePath)
        {
            PType = PieceType.Knight;
        }

        public override ulong Search(ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            ulong northNorthEast = (this.Position & MaskNotInHFile) << 15;
            ulong northEastEast = (this.Position & MaskNotInGHFile) << 6;
            ulong northNorthWest = (this.Position & MaskNotInAFile) << 17;
            ulong northWestWest = (this.Position & MaskNotInABFile) << 10;
            ulong southEastEast = (this.Position & MaskNotInGHFile) >> 10;
            ulong southSouthEast = (this.Position & MaskNotInHFile) >> 17;
            ulong southSouthWest = (this.Position & MaskNotInAFile) >> 15;
            ulong southWestWest = (this.Position & MaskNotInABFile) >> 6;
            ulong allDirection = northNorthEast ^ northEastEast ^ northNorthWest ^ northWestWest ^ southEastEast ^ southSouthEast ^ southWestWest ^ southSouthWest;
            ulong allPossibleMove = allDirection & ~ourPositions;
            return allPossibleMove;
        }

       
        public override ulong GetSpecificAttackFromSearch(ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions, ulong opponentPiecePosition)
        {
            ulong[] allMoves = new ulong[8];
            ulong northNorthEast = (this.Position & MaskNotInHFile) << 15;
            ulong northEastEast = (this.Position & MaskNotInGHFile) << 6;
            ulong northNorthWest = (this.Position & MaskNotInAFile) << 17;
            ulong northWestWest = (this.Position & MaskNotInABFile) << 10;
            ulong southEastEast = (this.Position & MaskNotInGHFile) >> 10;
            ulong southSouthEast = (this.Position & MaskNotInHFile) >> 17;
            ulong southSouthWest = (this.Position & MaskNotInAFile) >> 15;
            ulong southWestWest = (this.Position & MaskNotInABFile) >> 6;
            allMoves[0] = northNorthEast;
            allMoves[1] = northEastEast;
            allMoves[2] = northNorthWest;
            allMoves[3] = northWestWest;
            allMoves[4] = southEastEast;
            allMoves[5] = southSouthEast;
            allMoves[6] = southSouthWest;
            allMoves[7] = southWestWest;
            for (int i = 0; i < allMoves.Length; i++)
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

using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class King : BasePiece
    {
        private ulong MaskNotInAFile = 0b_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111;
        private ulong MaskNotInHFile = 0b_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110;
        public King OpponentKing { get; set; }

        public King(Player player, ColorSide color, ulong position, string boardName): base(player,color, position, boardName)
        {
            Name = "King";
        }

        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            ulong northAttack = currentPosition << 8;
            ulong northEastAttack = (currentPosition << 7 ) & MaskNotInAFile;
            ulong northWestAttack = (currentPosition << 9) & MaskNotInHFile;
            ulong westAttack = (currentPosition << 1 ) & MaskNotInHFile;
            ulong eastAttack = (currentPosition >> 1 ) & MaskNotInAFile;
            ulong southEastAttack = (currentPosition >> 9 ) & MaskNotInAFile;
            ulong southWestAttack =(currentPosition >> 7) & MaskNotInHFile;
            ulong southAttack = currentPosition >> 8;

            // so basically we need the other king position to keep 2 square distance between the kings as the rule say
            ulong opponentKingOpportunities = OpponentKing.Search(OpponentKing.Position, allPositionAtBoard, ourPositions, opponentPositionAtBoard);
            ulong allAttacks = (northAttack ^ northEastAttack ^ northWestAttack ^ westAttack ^ eastAttack ^ southEastAttack ^ southWestAttack ^ southAttack) & ~opponentKingOpportunities;
            //Printboard(Convert.ToString((long)allAttacks, toBase: 2).PadLeft(64, '0'));
            ulong allPossibilities = (allAttacks & ~ourPositions);
            return allPossibilities;
        }


    }
}

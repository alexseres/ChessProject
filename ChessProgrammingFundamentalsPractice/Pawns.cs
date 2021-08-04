using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class Pawns : BasePiece
    {
        public const int MovingDirection = 8;
        public readonly int[] AttacksDirection = new int[2] { 7, 9 };
        public readonly ulong LastLine;
        public readonly ulong FifthLineOfEnPassant;
        public Pawns OpponentPawns;

        //these mask we need if our pawn wants to attack and its at the sides, so therefore it cannot move onto the edge and jumping to another row
        public const ulong maskNotAColumn = 0b_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111;
        public const ulong maskNotHColumn = 0b_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110;

        //this mask checks if pawns at the starting position, and if they are, they got the chance to move 2 square
        public readonly ulong MaskOfDoubleMove;

        public Pawns(ColorSide color, ulong positions, ulong lastline) : base(color, positions)
        {
            LastLine = lastline;
            MaskOfDoubleMove = color == ColorSide.Black ? (ulong)0b_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000 : 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000;
            FifthLineOfEnPassant = color == ColorSide.Black ? (ulong)0b_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000 : 0b_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000;
        }
         

        public ulong DoEnPassant(ulong currentPositon, ulong opponentPositons, ulong ourPositions)
        {
            if ((currentPositon & FifthLineOfEnPassant) > 0)
            {
                //we need these mask to check if the latest move of the last enemy piece was a double move
                ulong seventhRank = 0b_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
                ulong fithRank = 0b_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000;
                ulong fourthRank = 0b_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000;
                ulong secondRank = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000;
                if ((((OpponentPawns.LatestMove.Item1 & seventhRank) > 0) && ((OpponentPawns.LatestMove.Item2 & fithRank)) > 0))
                {
                    if (((currentPositon & maskNotHColumn) >> 1 & (OpponentPawns.Positions)) > 0)
                    {
                        return currentPositon << 7;
                    }
                    else if (((currentPositon & maskNotAColumn) << 1 & (OpponentPawns.Positions)) > 0)
                    {
                        return currentPositon << 9;
                    }
                }
                else if((((OpponentPawns.LatestMove.Item1 & secondRank) > 0) && (OpponentPawns.LatestMove.Item2 & fourthRank) > 0))
                {
                    if (((currentPositon & maskNotHColumn) >> 1 & (OpponentPawns.Positions)) > 0)
                    {
                        return currentPositon >> 9;
                    }
                    else if (((currentPositon & maskNotAColumn) << 1 & (OpponentPawns.Positions)) > 0)
                    {
                        return currentPositon >> 7;
                    }
                }
            }
            return 0;
        }

        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            if(this.Color == ColorSide.Black)
            {
                
                ulong attackPositions = ((currentPosition >> AttacksDirection[1]) & maskNotAColumn) ^ ((currentPosition >> AttacksDirection[0]) & maskNotHColumn);
                //this one checks if we want to move double square and noone is in front of us
                ulong movedFirstPositions = (((MaskOfDoubleMove & currentPosition) >> 8) & ~allPositionAtBoard) >> 8;
                ulong movedPositions = (currentPosition >> MovingDirection) | movedFirstPositions;
                ulong opportunities = (~allPositionAtBoard & movedPositions) | ((~ourPositions & attackPositions) & opponentPositionAtBoard);
                return opportunities;
            }
            else
            {
                ulong attackPositions = ((currentPosition << AttacksDirection[0]) & maskNotAColumn) ^ ((currentPosition <<  AttacksDirection[1]) & maskNotHColumn);
                //this one checks if we want to move double square and noone is in front of us
                ulong movedFirstPositions = (((MaskOfDoubleMove & currentPosition) << 8) & ~allPositionAtBoard) << 8;
                ulong movedPositions = (currentPosition << MovingDirection) | movedFirstPositions;
                ulong opportunities = (~allPositionAtBoard & movedPositions) | ((~ourPositions & attackPositions) & opponentPositionAtBoard);
                return opportunities;
            }
        }


        public ulong SearchForOnlyAttack(ColorSide color,ulong currentPosition, ulong ourPositions, ulong opponentPiecePosition)
        {
            if(color == ColorSide.Black)
            {
                ulong attackPositions = ((currentPosition >> AttacksDirection[1]) & maskNotAColumn) ^ ((currentPosition >> AttacksDirection[0]) & maskNotHColumn);
                ulong opportunities = (~ourPositions & attackPositions) & opponentPiecePosition;
                return opportunities;
            }
            else
            {
                ulong attackPositions = ((currentPosition << AttacksDirection[0]) & maskNotAColumn) ^ ((currentPosition << AttacksDirection[1]) & maskNotHColumn);
                ulong opportunities = (~ourPositions & attackPositions) & opponentPiecePosition;
                return opportunities;
            }
        }
 

    }
}

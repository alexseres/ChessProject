using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.Models.Pieces
{
    [Serializable]
    public class Pawn : BasePiece
    {
        public const int MovingDirection = 8;
        public readonly int[] AttacksDirection = new int[2] { 7, 9 };
        public readonly ulong LastLine;
        public readonly ulong FifthLineOfEnPassant;
        public bool WasEnPassant { get; set; }

        //these mask we need if our pawn wants to attack and its at the sides, so therefore it cannot move onto the edge and jumping to another row
        public const ulong maskNotAColumn = 0b_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111;
        public const ulong maskNotHColumn = 0b_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110;

        //this mask checks if pawns at the starting position, and if they are, they got the chance to move 2 square
        public readonly ulong MaskOfDoubleMove;

        public Pawn(Player player, ColorSide color, ulong position, string boardName, ulong lastline, ulong maskOfDoubleMove, ulong fifthLineOfEnPassant) : base(player, color, position, boardName)
        {
            Name = "Pawn";
            WasEnPassant = false;
            LastLine = lastline;
            MaskOfDoubleMove = maskOfDoubleMove;
            FifthLineOfEnPassant = fifthLineOfEnPassant;
        }

        public ulong DoEnPassant(ulong currentPositon, ulong opponentPositons, ulong ourPositions)
        {
            ulong enPassant = 0;
            if ((currentPositon & FifthLineOfEnPassant) > 0)
            {
                //we need these mask to check if the latest move of the last enemy piece was a double move
                ulong seventhRank = 0b_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
                ulong fithRank = 0b_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000;
                ulong fourthRank = 0b_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000;
                ulong secondRank = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000;
                foreach (Pawn pawn in Creator.OpponentPawnsList)
                {

                    if ((((pawn.LatestMove.Item1 & seventhRank) > 0) && ((pawn.LatestMove.Item2 & fithRank)) > 0))
                    {
                        if (((currentPositon & maskNotHColumn) >> 1 & (pawn.Position)) > 0)
                        {
                            enPassant = currentPositon >> 1;
                        }
                        else if (((currentPositon & maskNotAColumn) << 1 & (pawn.Position)) > 0)
                        {
                            enPassant = currentPositon << 1;
                        }
                    }
                    else if ((((pawn.LatestMove.Item1 & secondRank) > 0) && (pawn.LatestMove.Item2 & fourthRank) > 0))
                    {
                        if (((currentPositon & maskNotHColumn) >> 1 & (pawn.Position)) > 0)
                        {
                            enPassant = currentPositon >> 1;
                        }
                        else if (((currentPositon & maskNotAColumn) << 1 & (pawn.Position)) > 0)
                        {
                            enPassant = currentPositon << 1;
                        }
                    }
                }
                return enPassant;
            }
            return 0;
        }

        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            if (this.Color == ColorSide.Black)
            {
                ulong enpassant = DoEnPassant(currentPosition, opponentPositionAtBoard, ourPositions);
                ulong attackPositions = ((currentPosition >> AttacksDirection[1]) & maskNotAColumn) ^ ((currentPosition >> AttacksDirection[0]) & maskNotHColumn);
                //this one checks if we want to move double square and noone is in front of us
                ulong movedFirstPositions = (((MaskOfDoubleMove & currentPosition) >> 8) & ~allPositionAtBoard) >> 8;
                ulong movedPositions = (currentPosition >> MovingDirection) | movedFirstPositions;
                ulong opportunities = (~allPositionAtBoard & movedPositions) | ((~ourPositions & attackPositions) & opponentPositionAtBoard);
                opportunities = opportunities | enpassant;
                return opportunities;
            }
            else
            {
                ulong enpassant = DoEnPassant(currentPosition, opponentPositionAtBoard, ourPositions);
                ulong attackPositions = ((currentPosition << AttacksDirection[0]) & maskNotAColumn) ^ ((currentPosition << AttacksDirection[1]) & maskNotHColumn);
                //this one checks if we want to move double square and noone is in front of us
                ulong movedFirstPositions = (((MaskOfDoubleMove & currentPosition) << 8) & ~allPositionAtBoard) << 8;
                ulong movedPositions = (currentPosition << MovingDirection) | movedFirstPositions;
                ulong opportunities = (~allPositionAtBoard & movedPositions) | ((~ourPositions & attackPositions) & opponentPositionAtBoard);
                opportunities = opportunities | enpassant;
                Printboard(Convert.ToString((long)opportunities, toBase: 2).PadLeft(64, '0'));
                return opportunities;
            }
        }

        public ulong SearchForOnlyAttack(ColorSide color, ulong currentPosition, ulong ourPositions, ulong opponentPiecePosition)
        {
            if (color == ColorSide.Black)
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

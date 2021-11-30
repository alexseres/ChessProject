using ChessProject.Models.Enums;
using ChessProject.Models.ObserverRelated;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ChessProject.Models.Pieces
{
    public class King : BasePiece
    {
        private ulong MaskNotInAFile = 0b_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111;
        private ulong MaskNotInHFile = 0b_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110;
        public King OpponentKing { get; set; }

        public King(Player player, ColorSide color, ulong position, string imagePath) : base(player, color, position, imagePath)
        {
            PType = PieceType.King;
        }

        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            ulong attacks = GetAllAttacks(currentPosition);
            // so basically we need the other king position to keep 2 square distance between the kings as the rule say
            ulong opponentKingOpportunities = OpponentKing.GetAllAttacks(OpponentKing.Position);
            ulong possibleAttacks = attacks & ~opponentKingOpportunities;
            //Printboard(Convert.ToString((long)allAttacks, toBase: 2).PadLeft(64, '0'));
            ulong opponentAttacks = GetFreeSquareWHereEnemyCannotIndave(allPositionAtBoard, opponentPositionAtBoard, ourPositions, Creator.OpponentPiecesList);

            ulong castlingAvailable = Creator.PlayerInCheck == false ? CheckForCastling(allPositionAtBoard) : 0;
            ulong allPossibilities = ((possibleAttacks & ~ourPositions) & ~opponentAttacks) | castlingAvailable;
            return allPossibilities;
        }


        public ulong GetAllAttacks(ulong currentPosition)
        {
            ulong northAttack = currentPosition << 8;
            ulong northEastAttack = (currentPosition << 7) & MaskNotInAFile;
            ulong northWestAttack = (currentPosition << 9) & MaskNotInHFile;
            ulong westAttack = (currentPosition << 1) & MaskNotInHFile;
            ulong eastAttack = (currentPosition >> 1) & MaskNotInAFile;
            ulong southEastAttack = (currentPosition >> 9) & MaskNotInAFile;
            ulong southWestAttack = (currentPosition >> 7) & MaskNotInHFile;
            ulong southAttack = currentPosition >> 8;
            return northAttack ^ northEastAttack ^ northWestAttack ^ westAttack ^ eastAttack ^ southEastAttack ^ southWestAttack ^ southAttack;
        }

        public bool CanKingGetAwayFromCheck(ulong allOpponentAttacks,ulong attackerPos,ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            //PrintBoard(Convert.ToString((long)allOpponentAttacks, toBase: 2).PadLeft(64, '0'));
            ulong moves = Search(Position, allPositionAtBoard, opponentPositionAtBoard, ourPositions);
            //PrintBoard(Convert.ToString((long)moves, toBase: 2).PadLeft(64, '0'));
            //Debug.WriteLine(" ");
            //PrintBoard(Convert.ToString((long)attackerPositionAndAttackVektor, toBase: 2).PadLeft(64, '0'));
            if (((moves & ~allOpponentAttacks) > 0) || (moves & attackerPos) > 0) return true; else return false;
        }

        public ulong CheckForCastling(ulong allPiecePositions)
        {
            if (this.LatestMove == (0, 0))
            {
                ulong result = 0;
                foreach (IObserver observer in Creator.PiecesList)
                {
                    BasePiece piece = observer as BasePiece;
                    if (piece is Rook)
                    {
                        Rook rook = piece as Rook;
                        if (rook.LatestMove == (0, 0))
                        {
                            int counterOfSquareDistance = 0;
                            bool occupied = false;
                            //checks if which is bigger and then we do iteration and we check if between the two piece any position is occupied
                            if (this.Position > rook.Position)
                            {
                                for (ulong i = rook.Position << 1; i < this.Position; i <<= 1)
                                {
                                    if ((i & (allPiecePositions)) > 0)
                                    {
                                        occupied = true;
                                        break;
                                    }
                                    counterOfSquareDistance++;
                                }
                                result |= occupied == false ? rook.Position : 0;
                            }
                            else
                            {
                                for (ulong k = this.Position << 1; k < rook.Position; k <<= 1)
                                {
                                    if ((k & (allPiecePositions)) > 0)
                                    {
                                        occupied = true;
                                        break;
                                    }
                                    counterOfSquareDistance++;
                                }
                                result |= occupied == false ? rook.Position : 0;
                            }

                        }
                    }
                }
                return result;
            }
            return 0;
        }

        public ulong GetFreeSquareWHereEnemyCannotIndave(ulong allPiecePositions, ulong opponentPositions, ulong ourPositions, List<IObserver> pieceListOfOpponent)
        {
            ulong mask = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            ulong attacks = 0;
            ulong allPawnAttacks = 0;
            for (int i = 0; i < 64; i++)
            {
                if ((opponentPositions & mask) > 0)
                {
                    foreach (IObserver observer in pieceListOfOpponent)
                    {
                        BasePiece piece = observer as BasePiece;
                        if (((piece.Position & mask) > 0) && !(piece is King))
                        {
                            ulong newAttack = piece.Search(mask, allPiecePositions, opponentPositions, ourPositions);
                            if(piece is Pawn)
                            {
                                Pawn pawn = piece as Pawn;
                                newAttack = pawn.SearchForOnlyAttack(mask, ourPositions, opponentPositions);
                                //PrintBoard(Convert.ToString((long)newAttack, toBase: 2).PadLeft(64, '0'));
                                allPawnAttacks |= newAttack;
                            }
                            attacks |= newAttack;
                        }
                    }
                }
                mask = mask >> 1;
            }
            PrintBoard(Convert.ToString((long)allPawnAttacks, toBase: 2).PadLeft(64, '0'));
            PrintBoard(Convert.ToString((long)attacks, toBase: 2).PadLeft(64, '0'));
            return attacks;
        }

        public void PrintBoard(string board)
        {
            Debug.WriteLine("Bishop");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < board.Length; i++)
            {
                if (i % 8 == 0 && i != 0)
                {
                    string row = new string(sb.ToString());
                    Debug.WriteLine(row);
                    sb.Clear();
                }
                sb.Append(board[i]);
            }
            var finalRow = new string(sb.ToString());
            Debug.WriteLine(finalRow);
            Debug.WriteLine(" ");
        }

    }
}

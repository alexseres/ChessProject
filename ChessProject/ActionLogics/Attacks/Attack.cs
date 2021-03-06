
using ChessProject.ActionLogics.Attacks;
using ChessProject.ActionLogics.BitBoardsUpdater;

using ChessProject.Models.ObserverRelated;
using ChessProject.Models.Pieces;
using ChessProject.Utils.BitScanLogic;
using ChessProject.Utils.PopulationCountLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ChessProject.ActionLogics
{
    [Serializable]
    public class Attack : IAttack
    {
        public IBitScan Scan { get; set; }
        public IPopulationCount PopCount { get; set; }
        public IUpdateBitBoards BitBoardsUpdater { get; set; }

        public Attack(IBitScan bitScan, IPopulationCount popCount, IUpdateBitBoards updater)
        {
            Scan = bitScan;
            PopCount = popCount;
            BitBoardsUpdater = updater;
        }

        public bool CheckMateChecker(ulong attackerPieceRoute, ulong oldPosOfDefenderPiece, ulong defenderPieceRoute, ulong kingPosition, ulong allPiecePositions, ulong opponentPositions, ulong ourPositions, List<IObserver> opponentPieceList)
        {

            ulong union = attackerPieceRoute & defenderPieceRoute;
            int population = PopCount.GetPopulation(union);
            for (int i = 0; i < population; i++)         // most of the time it is just one iteration
            {
                int pos = Scan.bitScanForwardLS1B(union);
                ulong movedPos = ((ulong)1 << pos);
                List<BasePiece> mighChangedOpponentPieceList = BitBoardsUpdater.SeparateUpdatePieceList(opponentPieceList, movedPos);
                ulong[] changedPositions = BitBoardsUpdater.SeparateUpdateBitBoardsToEvadeCheck(movedPos, oldPosOfDefenderPiece, defenderPieceRoute, allPiecePositions, ourPositions, opponentPositions);
                ulong IsKingStillInCheck = GetOpponentAttackToCheckIfKingInCheckIfThereIs(kingPosition, changedPositions[0], changedPositions[1], changedPositions[2], mighChangedOpponentPieceList);
                if (IsKingStillInCheck == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool GetCounterAttackToChekIfSomePieceCouldEvadeAttack(ulong attackerPositionAndAttackVektor, ulong kingPosition, ulong allPiecePositions, ulong opponentPositions, ulong ourPositions, List<IObserver> ourPieceList, List<IObserver> opponentPieceList)
        {
            //ulong mask = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            //Debug.WriteLine("Attaker pos and vector");
            //Printboard(Convert.ToString((long)attackerPositionAndAttackVektor, toBase: 2).PadLeft(64, '0'));
            //for (int i = 0; i < 64; i++)
            //{
            //    if ((ourPositions & mask) > 0)
            //    {
            //        foreach (IObserver observer in ourPieceList)
            //        {
            //            BasePiece piece = observer as BasePiece;
            //            if ((piece.Position & mask) > 0)   //it can defend it
            //            {
            //                ulong counterAttack = piece.Search(mask, allPiecePositions, opponentPositions, ourPositions);  // here we replaced two arguments(our <-> opp)
            //                Printboard(Convert.ToString((long)counterAttack, toBase: 2).PadLeft(64, '0'));
            //                if ((counterAttack & attackerPositionAndAttackVektor) > 0)
            //                {
            //                    bool IsKingStilInCheck = CheckMateChecker(attackerPositionAndAttackVektor, mask, counterAttack, kingPosition, allPiecePositions, opponentPositions, ourPositions, opponentPieceList);
            //                    if (!IsKingStilInCheck)
            //                    {
            //                        return false;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    mask = mask >> 1;
            //}
            foreach (IObserver observer in ourPieceList)
            {
                BasePiece piece = observer as BasePiece;

                ulong counterAttack = piece.Search(allPiecePositions, opponentPositions, ourPositions);  // here we replaced two arguments(our <-> opp)
                Printboard(Convert.ToString((long)counterAttack, toBase: 2).PadLeft(64, '0'));
                if ((counterAttack & attackerPositionAndAttackVektor) > 0)
                {
                    bool IsKingStilInCheck = CheckMateChecker(attackerPositionAndAttackVektor, piece.Position, counterAttack, kingPosition, allPiecePositions, opponentPositions, ourPositions, opponentPieceList);
                    if (!IsKingStilInCheck)
                    {
                        return false;
                    }
                }
               
            }
            return true;
        }

        public ulong GetAllOpponentAttack(ulong allPiecePositions, ulong opponentPositions, ulong ourPositions, List<BasePiece> pieceListOfOpponent)
        {
            ulong allAttack = 0;

            foreach (BasePiece piece in pieceListOfOpponent)
            {
                if (piece is Pawn)
                {
                    Pawn pawn = piece as Pawn;
                    allAttack |= pawn.SearchForOnlyAttack(opponentPositions, ourPositions); // we add mask because if we do a counter attack we must know the enemyposition too
                }
                else
                {
                    allAttack |= piece.Search(allPiecePositions, ourPositions, opponentPositions);  // here we replaced two arguments(our <-> opp) // here we replaced two arguments(our <-> opp)
                }
            }
            return allAttack;
        }


        public ulong GetOpponentAttackToCheckIfKingInCheckIfThereIs(ulong kingPosition, ulong allPiecePositions, ulong opponentPositions, ulong ourPositions, List<BasePiece> pieceListOfOpponent)
        {
            //ulong allAttacks = 0;
            //ulong mask = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            //for (int i = 0; i < 64; i++)
            //{
            //    if ((opponentPositions & mask) > 0)
            //    {
            //        foreach (BasePiece piece in pieceListOfOpponent)
            //        {
            //            if ((piece.Position & mask) > 0)
            //            {
            //                if (piece is Pawn)
            //                {
            //                    Pawn pawn = piece as Pawn;
            //                    ulong newAttack = pawn.SearchForOnlyAttack(mask, opponentPositions, kingPosition) | mask; // we add mask because if we do a counter attack we must know the enemyposition too
            //                    allAttacks = allAttacks | newAttack;
            //                    if ((newAttack & kingPosition) > 0)
            //                    {
            //                        return newAttack;
            //                    }
            //                    //attacks = attacks | newAttack;
            //                }
            //                else
            //                {
            //                    //Printboard(Convert.ToString((long)piece.Position, toBase: 2).PadLeft(64, '0'));
            //                    ulong newAttack = piece.GetSpecificAttackFromSearch(mask, allPiecePositions, ourPositions, opponentPositions, kingPosition) | mask;  // here we replaced two arguments(our <-> opp) // here we replaced two arguments(our <-> opp)
            //                    allAttacks = allAttacks | newAttack;
            //                    if ((newAttack & kingPosition) > 0)
            //                    {
            //                        return newAttack;
            //                    }
            //                    //attacks |= newAttack;
            //                }
            //                break;
            //            }
            //        }
            //    }
            //    mask = mask >> 1;
            //}


            foreach (BasePiece piece in pieceListOfOpponent)
            {
                if (piece is Pawn)
                {
                    Pawn pawn = piece as Pawn;
                    ulong newAttack = pawn.SearchForOnlyAttack(opponentPositions, kingPosition) | piece.Position; // we add mask because if we do a counter attack we must know the enemyposition too
                    if ((newAttack & kingPosition) > 0)
                    {
                        return newAttack;
                    }
                }
                else
                {
                    ulong newAttack = piece.GetSpecificAttackFromSearch(allPiecePositions, ourPositions, opponentPositions, kingPosition) | piece.Position;  // here we replaced two arguments(our <-> opp) // here we replaced two arguments(our <-> opp)
                    if ((newAttack & kingPosition) > 0)
                    {
                        return newAttack;
                    }
                }
            }
            return 0;
        }



        public ulong GetRayAttacks(ulong allPositionAtBoard, ulong opponent, int square, Func<int, ulong> rayAttack, Func<ulong, int> bitScan, int direction)
        {
            ulong attacks = rayAttack(square);
            ulong blocker = attacks & allPositionAtBoard;
            if (blocker > 0)
            {
                square = bitScan(blocker);
                ulong squarePosition = ((ulong)1 << square);
                if ((opponent & squarePosition) > 0)
                {
                    //this includes the actual line
                }
                else if (((allPositionAtBoard & ~opponent) & squarePosition) > 0)
                {
                    square += SetBitScanSubtracter(direction);
                }
                attacks = (attacks & ~rayAttack(square));
            }
            return attacks;
        }

        public ulong GetAttackerPos(ulong attackerAttackVectorAndCurrentPos, List<BasePiece> pieceListOfOpponent)
        {
            return pieceListOfOpponent.Where(x => ((x.Position & attackerAttackVectorAndCurrentPos) > 0)).First().Position;
        }


        public bool HasAttacked(ulong pos, ulong opponentPositions)
        {
            return (pos & opponentPositions) > 0 ? true : false;
        }

        public void Printboard(string board)
        {
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
            var finalrow = new string(sb.ToString());
            Debug.WriteLine(finalrow);
            Debug.WriteLine(" ");

        }

        public int SetBitScanSubtracter(int num)
        {
            switch (num)
            {
                case 8: //North
                    return -8;
                case 1: //West
                    return -1;
                case -8: //South
                    return 8;
                case -1: //East
                    return 1;
                case 9: //WestNorth
                    return -9;
                case 7: //EastNorth
                    return -7;
                case -7: //WestSouth
                    return 7;
                case -9: // EastSouth
                    return 9;

                default:
                    throw new Exception("wrong direction code");
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class Attack : IAttack
    {
        public bool CheckMateChecker(ulong attackerPieceRoute ,ulong oldPosOfDefenderPiece, ulong defenderPieceRoute, ulong kingPosition, ulong allPiecePositions, ulong opponentPositions, ulong ourPositions, List<IObserver> opponentPieceList)
        {
            BitScan bitScan = new BitScan();
            PopulationCount populationCounter = new PopulationCount();
            UpdateBitBoards updateBitBoards = new UpdateBitBoards();
            ulong union = attackerPieceRoute & defenderPieceRoute;
            int population = populationCounter.GetPopulation(union);    
            for(int i = 0;i < population; i++)         // most of the time it is just one iteration
            {
                int pos = bitScan.bitScanForwardLS1B(union);
                ulong movedPos = ((ulong)1 << pos);
                List<IObserver> mighChangedOpponentPieceList =  updateBitBoards.SeparateUpdatePieceList(opponentPieceList, movedPos);
                ulong[] changedPositions =  updateBitBoards.SeparateUpdateBitBoardsToEvadeCheck(movedPos, oldPosOfDefenderPiece, defenderPieceRoute, allPiecePositions, ourPositions, opponentPositions);
                ulong IsKingStillInCheck = GetAllOpponentAttackToCheckIfKingInCheck(kingPosition, changedPositions[0], changedPositions[1], changedPositions[2], mighChangedOpponentPieceList);
                if(IsKingStillInCheck == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool GetCounterAttackToChekIfSomePieceCouldEvadeAttack(ulong attackerPositionAndAttackVektor,ulong kingPosition, ulong allPiecePositions, ulong opponentPositions, ulong ourPositions, List<IObserver> ourPieceList, List<IObserver> opponentPieceList)
        {   
            ulong mask = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            ulong attacks = 0;
            for (int i = 0; i < 64; i++)
            {
                if ((ourPositions & mask) > 0)
                {
                    foreach(IObserver observer in ourPieceList)
                    {
                        BasePiece piece = observer as BasePiece;
                        if ((piece.Position & mask) > 0)   //it can defend it
                        {
                            ulong counterAttack = piece.Search(mask, allPiecePositions,opponentPositions, ourPositions);  // here we replaced two arguments(our <-> opp)
                            if ((counterAttack & attackerPositionAndAttackVektor) > 0)
                            {
                                bool IsKingStilInCheck = CheckMateChecker(attackerPositionAndAttackVektor, mask,counterAttack, kingPosition, allPiecePositions, opponentPositions, ourPositions, opponentPieceList);
                                if (!IsKingStilInCheck)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                mask = mask >> 1;
            }
            return true;
        }


        public ulong GetAllOpponentAttackToCheckIfKingInCheck(ulong kingPosition,ulong allPiecePositions, ulong opponentPositions, ulong ourPositions,  List<IObserver> pieceListOfOpponent)
        {
            ulong mask = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            ulong attacks = 0;
            for (int i = 0; i < 64; i++)
            {
                if((opponentPositions & mask) > 0)
                {
                    foreach (IObserver observer in pieceListOfOpponent)
                    {
                        BasePiece piece = observer as BasePiece;
                        if ((piece.Position & mask) > 0)
                        {
                            if(piece is Pawns)
                            {
                                Pawns pawn = piece as Pawns;
                                ulong newAttack = pawn.SearchForOnlyAttack(pawn.Color, mask, opponentPositions, kingPosition) | mask; // we add mask because if we do a counter attack we must know the enemyposition too
                                if ((newAttack & kingPosition) > 0)
                                { 
                                    return newAttack;
                                } 
                                //attacks = attacks | newAttack;
                            }
                            else
                            {
                                ulong newAttack = piece.GetSpecificAttackFromSearch(mask, allPiecePositions, ourPositions, opponentPositions, kingPosition) | mask;  // here we replaced two arguments(our <-> opp) // here we replaced two arguments(our <-> opp)
                                if ((newAttack & kingPosition) > 0)
                                {
                                    return newAttack; 
                                }
                                //attacks |= newAttack;
                            }
                            break;
                        }
                    }
                }
                mask = mask >> 1;
            }
            return 0;
        }

        public ulong GetRayAttacks(ulong allPositionAtBoard, ulong opponent, int square, Func<int, ulong> rayAttack, Func<ulong, int> bitScan, int direction)
        {
            ulong attacks = rayAttack(square);
            //Printboard(Convert.ToString((long)attacks, toBase: 2).PadLeft(64, '0'));

            ulong blocker = attacks & allPositionAtBoard;
            //Printboard(Convert.ToString((long)blocker, toBase: 2).PadLeft(64, '0'));
            if (blocker > 0)
            {
                square = bitScan(blocker);
                ulong squarePosition = ((ulong)1 << square);
                if ((opponent &  squarePosition) > 0)
                {
                    //this includes the actual line
                }
                else if(((allPositionAtBoard & ~opponent) & squarePosition) > 0 )
                {
                    square += SetBitScanSubtracter(direction); 
                }
                attacks = (attacks & ~rayAttack(square));
            }
            return attacks;
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
                    Console.WriteLine(row);
                    sb.Clear();
                }
                sb.Append(board[i]);
            }
            var finalrow = new string(sb.ToString());
            Console.WriteLine(finalrow);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class ProofOfConcept
    {
        public readonly Player Player1;
        public readonly Player Player2;
        public const string From = "Enter position to choose piece";
        public const string To = "Enter position to choose piece";


        public string[] BlackPrintedBoardNames = new string[6] { "R", "K", "B", "Q", "N", "P" };
        public ulong[] BlackInitPositions = new ulong[7] { 0b_1111_1111_1011_1111_0000_0000_0000_0000_0000_0000_0000_1000_0000_0000_0000_0000,
                                                           0b_1000_0001_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0100_0010_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0010_0100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0001_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0000_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0000_0000_1011_1111_0000_0000_0000_0000_0000_0000_0000_1000_0000_0000_0000_0000};

        public string[] WhitePrintedBoardNames = new string[6] { "A", "S", "D", "F", "G", "H" };
        public ulong[] WhiteInitPositions = new ulong[7] { 0b_0000_0000_0000_0000_0000_0000_0000_0000_1000_0000_0000_0010_1111_1111_1111_1111,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0010_0000_0000_1000_0001,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0100_0010,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0010_0100,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_1000_0000_0000_0000_0000_0000_0000_1000,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_0000,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000 };

        public ulong BoardWithAllMember = 0b_1111_1111_1011_1111_0000_0000_0000_0000_1000_0000_0100_1010_1111_1111_1111_1111;
  
        public ProofOfConcept()
        {
            Player1 = new Player(ColorSide.Black, BlackInitPositions, BlackPrintedBoardNames, new BitScan(), new LongMovements(), new RayAttack());
            Player2 = new Player(ColorSide.White, WhiteInitPositions, WhitePrintedBoardNames, new BitScan(), new LongMovements(), new RayAttack());
            string board = CreateStringOfBoard();
            PrintBoard(board);
            PlayGame();
        }

        public void PlayGame()
        {
            bool isWhiteAtTurn = true;
            while(true)
            {
                ulong choosenPos = UserInput(From);
                if (isWhiteAtTurn)
                {
                    BasePiece choosenWhitePiece = GrabAndExtractPiece(Player2, choosenPos);
                    bool response = Process(Player2, choosenWhitePiece, choosenPos);
                    if (response == true)
                    {
                        isWhiteAtTurn = false;
                    }
                    else
                    {
                        isWhiteAtTurn = true;
                    }
                }
                else
                {
                    BasePiece choosenBlackPiece = GrabAndExtractPiece(Player1, choosenPos);
                    bool response = Process(Player1, choosenBlackPiece, choosenPos);
                    if(response == true)
                    {
                        isWhiteAtTurn = true; 
                    }
                    else
                    {
                        isWhiteAtTurn = false;
                    }
                }
            }
        }
        
        public bool Process(Player player ,BasePiece piece,  ulong currentPiecePosition)
        {
            Player opponent = OpponentCreater(player);
            ulong opportunities = piece.Search(currentPiecePosition, BoardWithAllMember, opponent.Pieces, player.Pieces);
            string opportunitiesToString = Convert.ToString((long)opportunities, toBase: 2).PadLeft(64, '0');
            Console.WriteLine("opportunities");
            PrintBoard(opportunitiesToString);
            if (opportunities <= 0)
            {
                Console.WriteLine("you cannot move with this piece, choose another one");
                return false;
            }

            ulong choosenPositionToMove = UserInput(To);
            if((choosenPositionToMove & opportunities) <= 0)
            {
                Console.WriteLine("You cannot move there because there is no opportunity there");
                return false;
            }
            bool attacked = HasAttacked(choosenPositionToMove, opponent.Pieces);
            UpdateAllBitBoard(attacked, player, opponent, choosenPositionToMove, piece, opportunities, currentPiecePosition);
            
            Console.WriteLine("updated board");
            string updatedBoard = CreateStringOfBoard();
            PrintBoard(updatedBoard);
            return true;
        }

        public void UpdateAllBitBoard(bool attacked, Player currentPlayer, Player opponent, ulong choosenPositionToMove, BasePiece currentPiece, ulong opportunities, ulong currentPosition)
        {
            if (attacked)
            {
                BasePiece attackedPiece = GrabAndExtractPiece(opponent, choosenPositionToMove);
                attackedPiece.UpdatePositionWhenBeingAttacked(choosenPositionToMove);
                opponent.Pieces = opponent.Pieces & ~choosenPositionToMove;
            }
            currentPlayer.Pieces = (currentPlayer.Pieces & ~currentPiece.Positions);
            currentPiece.UpdatePositionWhenMove(currentPosition, opportunities, choosenPositionToMove);
            currentPlayer.Pieces = currentPlayer.Pieces ^ currentPiece.Positions;
            BoardWithAllMember = currentPlayer.Pieces ^ opponent.Pieces;
        }


        public bool HasAttacked(ulong pos, ulong opponentPositions)
        {
            return (pos & opponentPositions) > 0 ? true : false;
        }


        public Player OpponentCreater(Player player)
        {
            if(player == Player1)
            {
                return Player2;
            }
            else
            {
                return Player1;
            }
        }


        public BasePiece GrabAndExtractPiece(Player player, ulong pos)
        {
            
            Console.WriteLine(Convert.ToString((long)pos, toBase:2).PadLeft(64,'0'));
            for (int i = 0;i < player.Length; i++)
            {
                Console.WriteLine(Convert.ToString((long)Player2[i].Positions, toBase:2).PadLeft(64,'0'));
                if ((player.Pieces & (pos & player[i].Positions)) > 0)
                {
                    Console.WriteLine(" ");
                    return player[i];
                }
            }
            return null;
        }



        public ulong UserInput(string inp)
        {
            Console.WriteLine(inp);
            string result = Console.ReadLine();
            int convertToInt = Int16.Parse(result) - 1;
            ulong pos = (ulong)1 << convertToInt;
            return pos;
        }
       

        #region Print and Create Board
        public void PrintBoard(string board)
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
            var finalRow = new string(sb.ToString());
            Console.WriteLine(finalRow);
            
        }

        public string CreateStringOfBoard()
        {
            ulong mask = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            StringBuilder sb = new StringBuilder();
            for(int i = 0;i< 64;i++)
            {
                bool IsSquareOccupied = false;
                for(int j = 0; j < 6; j++)
                {
                    if((Player1[j].Positions & (mask & BoardWithAllMember)) > 0)
                    {
                        sb.Append(Player1[j].BoardName);
                        IsSquareOccupied = true;
                    }
                    else if ((Player2[j].Positions & (mask & BoardWithAllMember)) > 0)
                    {
                        sb.Append(Player2[j].BoardName);
                        IsSquareOccupied = true;
                    }
                }
                if (!IsSquareOccupied)
                {
                    sb.Append(".");
                }

                mask = mask >> 1;
            }
            Console.WriteLine(sb.ToString());
            return sb.ToString();
        }
        #endregion
    }
}
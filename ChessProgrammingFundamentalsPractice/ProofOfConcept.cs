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

        public string[] BlackPrintedBoardNames = new string[6] { "R", "K", "B", "Q", "N", "P" };
        public ulong[] BlackInitPositions = new ulong[7] { 0b_1111_1111_1011_1111_0000_0000_0000_0000_0000_0000_0000_0010_0000_0000_0000_0000,
                                                           0b_1000_0001_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0100_0010_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0010_0100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0001_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0000_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0000_0000_1011_1111_0000_0000_0000_0000_0000_0000_0000_0010_0000_0000_0000_0000};

        public string[] WhitePrintedBoardNames = new string[6] { "A", "S", "D", "F", "G", "H" };
        public ulong[] WhiteInitPositions = new ulong[7] { 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1000_0001,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0100_0010,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0010_0100,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1000,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_0000,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000 };

        public ulong BoardWithAllMember = 0b_1111_1111_1011_1111_0000_0000_0000_0000_0000_0000_0000_0010_1111_1111_1111_1111;
  
        public ProofOfConcept()
        {
            Player1 = new Player(ColorSide.Black, BlackInitPositions, BlackPrintedBoardNames);
            Player2 = new Player(ColorSide.White, WhiteInitPositions, WhitePrintedBoardNames);
            string board = CreateStringOfBoard();
            PrintBoard(board);
            PlayGame();
        }

        public void PlayGame()
        {
            bool isWhiteAtTurn = true;
            while(true)
            {
                if (isWhiteAtTurn)
                {
                    int choosenNum = ChoosePiece();
                    ulong pos = (ulong)1 << choosenNum;
                    BasePiece choosenWhitePiece = GrabAndExtractPiece(Player2, pos);
                    //basically just checks if the given pos is piece 
                    if (choosenWhitePiece is null) continue;
                    Process(Player2, choosenWhitePiece, pos);
                    isWhiteAtTurn = false;
                }
                else
                {
                    int pos = ChoosePiece();
                    //BasePiece choosenBlackPiece = GrabAndExtractPiece(Player1, pos);
                    //isWhiteAtTurn = true;
                }
            }
        }
        
        public void Process(Player player ,BasePiece piece,  ulong currentPiecePosition)
        {
            Player opponent = OpponentCreater(player);
            ulong opportunities = piece.Search(currentPiecePosition, BoardWithAllMember, opponent.Pieces, player.Pieces);
            string opportunitiesToString = Convert.ToString((long)opportunities, toBase: 2).PadLeft(64, '0');
            PrintBoard(opportunitiesToString);

            int choosenPositionToMove = WhereToGo();
            
            
        }

        public bool HasAttacked(int pos, ulong opponentPositions)
        {
            ulong convertedPos = (ulong)1 << pos;
            return (convertedPos & opponentPositions) > 0 ? true : false;
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



        public int WhereToGo()
        {
            Console.WriteLine("Enter position to where you wanna move the piece");
            string result = Console.ReadLine();
            int convertToInt = Int16.Parse(result) - 1;
            return convertToInt;
        }
        public int ChoosePiece()
        {
            Console.WriteLine("Enter position to choose piece");
            string result = Console.ReadLine();
            int convertToInt = Int16.Parse(result) - 1;
            return convertToInt;
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
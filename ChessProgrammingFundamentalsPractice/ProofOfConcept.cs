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
        public ulong[] BlackInitPositions = new ulong[7] { 0b_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                           0b_1000_0001_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0100_0010_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0010_0100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0001_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0000_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000};

        public string[] WhitePrintedBoardNames = new string[6] { "A", "S", "D", "F", "G", "H" };
        public ulong[] WhiteInitPositions = new ulong[7] { 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1000_0001,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0100_0010,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0010_0100,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1000,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_0000,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000 };

        public ulong BoardWithAllMember = 0b_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111;
  
        public ProofOfConcept()
        {
            Player1 = new Player(ColorSide.Black, BlackInitPositions, BlackPrintedBoardNames);
            Player2 = new Player(ColorSide.White, WhiteInitPositions, WhitePrintedBoardNames);
            string board = CreateStringOfBoard();
            PrintBoard(board);
        }

        public void PlayGame()
        {
            bool isWhiteAtTurn = true;
            while(true)
            {
                if (isWhiteAtTurn)
                {
                    ulong pos = AskUserInput();
                    isWhiteAtTurn = false;
                }
                else
                {
                    ulong pos = AskUserInput();
                    isWhiteAtTurn = true;
                }

            }
        }

        public void WhiteTurn(ulong pos)
        {


            for(int i = 0;i < Player2.Length; i++)
            {
                if((Player2.Pieces & (pos & Player2[i].Positions)) > 0)
                {

                }
/           }
        }

        public void BlackTurn(int pos)
        {

        }

        public ulong AskUserInput()
        {
            Console.WriteLine("Enter position");
            string result = Console.ReadLine();
            ulong converteToInt = (ulong)Int64.Parse(result);
            return converteToInt;
        }

        #region Print and Create Board
        public void PrintBoard(string board)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < board.Length; i++)
            {
                if (i % 8 == 0 && i != 0)
                {
                    string row = new string(sb.ToString().Reverse().ToArray());
                    Console.WriteLine(row);
                    sb.Clear();
                }
                sb.Append(board[i]);
            }
            var finalRow = new string(sb.ToString().Reverse().ToArray());
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
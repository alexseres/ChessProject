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

        public ulong BoardWithAllMember = 0b_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111;
        

        
        public void InitializePlayer(Player player)
        {
            switch (player.Color)
            {
                case ColorSide.Black:
                    player.Pieces = 0b_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
                    player.Rooks = 0b_1000_0001_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
                    player.Knights = 0b_0010_0100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
                    player.Bishops = 0b_0100_0010_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
                    player.Queen = 0b_0001_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
                    player.King = 0b_0000_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
                    player.Pawns = 0b_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
                    break;
                case ColorSide.White:
                    player.Pieces = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111;
                    player.Rooks = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1000_0001;
                    player.Knights = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0100_0010;
                    player.Bishops = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0010_0100;
                    player.Queen = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1000;
                    player.King = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_0000;
                    player.Pawns = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000;
                    break;
            }
        }

        public ProofOfConcept()
        {
        }

        public void PlayGame()
        {
            while (true)
            {
                      
            }
        }

        public int AskUserInput()
        {
            Console.WriteLine("Enter position");
            string result = Console.ReadLine();
            int converteToInt = Int16.Parse(result);
            return converteToInt;
        }

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
            var mask = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            StringBuilder sb = new StringBuilder();
            for(int i = 0;i< 64;i++)
            {   


                mask = mask >> 1;
            }
            Console.WriteLine(sb.ToString());
            return sb.ToString();
        }
    }
}

                //Console.WriteLine(tupl.Item1);
                //Console.WriteLine(Convert.ToString((long)BoardWithAllMember, toBase: 2).PadLeft(64, '0'));
                //Console.WriteLine(Convert.ToString((long)tupl.Item2, toBase: 2).PadLeft(64,'0'));
                //Console.WriteLine(Convert.ToString((long)(tupl.Item2 & BoardWithAllMember), toBase:2).PadLeft(64, '0')); 


                //if((BlackRooks & (mask & BoardWithAllMember)) > 0)
                //{
                //    sb.Append("R");
                //}
                //else if ((BlackKnights & (mask & BoardWithAllMember)) > 0)
                //{
                //    sb.Append("K");
                //}
                //else if ((BlackBishops & (mask & BoardWithAllMember)) > 0)
                //{
                //    sb.Append("B");
                //}
                //else if ((BlackQueen & (mask & BoardWithAllMember)) > 0)
                //{
                //    sb.Append("Q");
                //}
                //else if ((BlackKing & (mask & BoardWithAllMember)) > 0)
                //{
                //    sb.Append("N");
                //}
                //else if ((BlackPawns & (mask & BoardWithAllMember)) > 0)
                //{
                //    sb.Append("P");
                //}
                //else if ((WhiteRooks & (mask & BoardWithAllMember)) > 0)
                //{
                //    sb.Append("A");
                //}
                //else if ((WhiteKnights & (mask & BoardWithAllMember)) > 0)
                //{
                //    sb.Append("S");
                //}
                //else if ((WhiteBishops & (mask & BoardWithAllMember)) > 0)
                //{
                //    sb.Append("D");
                //}
                //else if ((WhiteQueen & (mask & BoardWithAllMember)) > 0)
                //{
                //    sb.Append("F");
                //}
                //else if ((WhiteKing & (mask & BoardWithAllMember)) > 0)
                //{
                //    sb.Append("G");
                //}
                //else if ((WhitePawns & (mask & BoardWithAllMember)) > 0)
                //{
                //    sb.Append("H");
                //}

                //else
                //{
                //    sb.Append(".");
                //}
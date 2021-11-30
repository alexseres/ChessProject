using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.Actions.Movements
{
    [Serializable]
    public class LongMovements : ILongMovements
    {
        public const ulong maskNotAColumn = 0b_1000_0000_1000_0000_1000_0000_1000_0000_1000_0000_1000_0000_1000_0000_1000_0000;
        public const ulong maskNotHColumn = 0b_0000_0001_0000_0001_0000_0001_0000_0001_0000_0001_0000_0001_0000_0001_0000_0001;

        public ulong GetNorth(int sq)
        {
            return (ulong)0x0101010101010100 << sq;
        }

        public ulong GetWest(int sq)
        {
            ulong one = 1;
            return 2 * ((one << (sq | 7)) - (one << sq));
        }

        public ulong GetSouth(int square)
        {
            return (ulong)0x0080808080808080 >> (square ^ 63);
        }

        public ulong GetEast(int square)
        {
            ulong one = 1;
            return (one << square) - (one << (square & 56));
        }

        public ulong GetWestNorth(int square)
        {
            ulong pos = (ulong)1 << square;
            ulong initPos = pos;
            if ((pos & maskNotAColumn) > 0)
            {
                return 0;
            }

            //Printboard(Convert.ToString((long)pos, toBase: 2).PadLeft(64, '0'));
            //Console.WriteLine(" ");
            for (int i = square + 9; i <= 64; i += 9)
            {
                if ((pos & maskNotAColumn) > 0)
                {
                    break;
                }
                pos = pos | (pos << 9);
                //Printboard(Convert.ToString((long)pos, toBase: 2).PadLeft(64, '0'));
                //Console.WriteLine(" ");
            }
            //Printboard(Convert.ToString((long)pos, toBase: 2).PadLeft(64, '0'));
            return pos & ~initPos;

        }

        public ulong GetEastNorth(int square)
        {
            ulong pos = (ulong)1 << square;
            ulong initPos = pos;
            if ((pos & maskNotHColumn) > 0)
            {
                return 0;
            }

            //Printboard(Convert.ToString((long)pos, toBase: 2).PadLeft(64, '0'));
            //Console.WriteLine(" ");
            for (int i = square + 7; i < 64; i += 7)
            {
                //Printboard(Convert.ToString((long)pos, toBase: 2).PadLeft(64, '0'));
                if ((pos & maskNotHColumn) > 0)
                {
                    //Printboard(Convert.ToString((long)pos, toBase: 2).PadLeft(64, '0'));
                    //pos = pos | (pos << 7);
                    //Printboard(Convert.ToString((long)pos, toBase: 2).PadLeft(64, '0'));
                    break;
                }
                pos = pos | (pos << 7);
                //Printboard(Convert.ToString((long)pos, toBase: 2).PadLeft(64, '0'));
                Console.WriteLine(" ");
            }
            //Printboard(Convert.ToString((long)pos, toBase: 2).PadLeft(64, '0'));
            return pos & ~initPos;
        }

        public ulong GetEastSouth(int square)
        {
            ulong pos = (ulong)1 << square;
            ulong initPos = pos;
            if ((pos & maskNotHColumn) > 0)
            {
                return 0;
            }

            for (int i = square - 9; i >= 0; i -= 9)
            {
                if ((pos & maskNotHColumn) > 0)
                {
                    break;
                }
                pos = pos | (pos >> 9);
            }

            return pos & ~initPos;
        }

        public ulong GetWestSouth(int square)
        {
            ulong pos = (ulong)1 << square;
            ulong initPos = pos;
            if ((pos & maskNotAColumn) > 0)
            {
                return 0;
            }

            for (int i = square - 7; i > 0; i -= 7)
            {
                if ((pos & maskNotAColumn) > 0)
                {
                    break;
                }
                pos = pos | (pos >> 7);
            }

            return pos & ~initPos;
        }

        public void Printboard(string board)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < board.Length; i++)
            {
                if (i % 8 == 0 && i != 0)
                {
                    string row = new string(sb.ToString());
                    //Console.WriteLine(row);
                    sb.Clear();
                }
                sb.Append(board[i]);
            }
            var finalrow = new string(sb.ToString());
            //Console.WriteLine(finalrow);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class LongMovements : ILongMovements
    {
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
    }
}

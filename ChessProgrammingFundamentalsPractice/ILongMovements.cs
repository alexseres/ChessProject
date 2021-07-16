using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    interface ILongMovements
    {
        ulong GetNorth(int square);
        ulong GetEast(int square);
        ulong GetSouth(int square);
        ulong GetWest(int square);
    }
}

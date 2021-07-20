using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public interface ILongMovements
    {
        ulong GetNorth(int square);
        ulong GetEast(int square);
        ulong GetSouth(int square);
        ulong GetWest(int square);
        ulong GetWestNorth(int square);
        ulong GetEastNorth(int square);

        ulong GetWestSouth(int suqare);
        ulong GetEastSouth(int square);
        

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public interface IRayAttack
    {
        public ulong GetRayAttacks(ulong allPositionAtBoard, ulong opponent, int square, Func<int, ulong> rayAttack, Func<ulong, int> bitScan, int direction);
    }
}

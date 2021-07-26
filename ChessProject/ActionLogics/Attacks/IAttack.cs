using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.ActionLogics.Attacks
{
    public interface IAttack
    {
        public ulong GetRayAttacks(ulong allPositionAtBoard, ulong opponent, int square, Func<int, ulong> rayAttack, Func<ulong, int> bitScan, int direction);
        public ulong KnightAttacks(ulong allPositionAtBoard, ulong opponent, int square, int direction);
    }
}

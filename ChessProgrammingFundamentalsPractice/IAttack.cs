﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public interface IAttack
    {
        public ulong GetRayAttacks(ulong allPositionAtBoard, ulong opponent, int square, Func<int, ulong> rayAttack, Func<ulong, int> bitScan, int direction);
        public ulong GetAllOpponentAttackToCheckIfKingInCheck(ulong kingPosition, ulong allPiecePositions, ulong opponentPositions, ulong ourPositions, List<IObserver> pieceListOfOpponent)
    }
}

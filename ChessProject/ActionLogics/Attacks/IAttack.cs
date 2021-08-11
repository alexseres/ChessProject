using ChessProject.Models.ObserverRelated;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.ActionLogics.Attacks
{
    public interface IAttack
    {
        public ulong GetRayAttacks(ulong allPositionAtBoard, ulong opponent, int square, Func<int, ulong> rayAttack, Func<ulong, int> bitScan, int direction);
        public bool GetCounterAttackToChekIfSomePieceCouldEvadeAttack(ulong attackerPositionAndAttackVektor, ulong kingPosition, ulong allPiecePositions, ulong opponentPositions, ulong ourPositions, List<IObserver> ourPieceList, List<IObserver> opponentPieceist);
        public ulong GetAllOpponentAttackToCheckIfKingInCheck(ulong kingPosition, ulong allPiecePositions, ulong opponentPositions, ulong ourPositions, List<IObserver> pieceListOfOpponent);
        public ulong GetAllOpponentAttackToCheckIfKingStillInCheck(ulong allPiecePositions, ulong opponentPositions, ulong ourPositions, List<IObserver> pieceListOfOpponent);

        public bool HasAttacked(ulong pos, ulong opponentPositions);
    }
}

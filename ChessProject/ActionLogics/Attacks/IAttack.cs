using ChessProject.Models.ObserverRelated;
using ChessProject.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.ActionLogics.Attacks
{
    public interface IAttack
    {
        public ulong GetRayAttacks(ulong allPositionAtBoard, ulong opponent, int square, Func<int, ulong> rayAttack, Func<ulong, int> bitScan, int direction);
        public bool GetCounterAttackToChekIfSomePieceCouldEvadeAttack(ulong attackerPositionAndAttackVektor, ulong kingPosition, ulong allPiecePositions, ulong opponentPositions, ulong ourPositions, List<IObserver> ourPieceList, List<IObserver> opponentPieceist);
        public ulong GetOpponentAttackToCheckIfKingInCheckIfThereIs(ulong kingPosition, ulong allPiecePositions, ulong opponentPositions, ulong ourPositions, List<BasePiece> pieceListOfOpponent);
        public ulong GetAllOpponentAttack(ulong allPiecePositions, ulong opponentPositions, ulong ourPositions, List<BasePiece> pieceListOfOpponent);
        public ulong GetAttackerPos(ulong attackerAttackVectorAndCurrentPos, List<BasePiece> pieceListOfOpponent);
        public bool HasAttacked(ulong pos, ulong opponentPositions);
    }
}

using ChessProject.Models;
using ChessProject.Models.ObserverRelated;
using ChessProject.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.ActionLogics.BitBoardsUpdater
{
    public interface IUpdateBitBoards
    {
        public void UpdateAllBitBoard(bool attacked, Player currentPlayer, Player opponent, ulong choosenPositionToMove, ulong opportunities, ulong currentPosition, ref ulong boardWithAllMember);
        public ulong[] SeparateUpdateBitBoardsToEvadeCheck(ulong newPositionOfDefender, ulong oldPositionOfDefender, ulong defenderPieceRoute, ulong allPiecePositions, ulong ourPositions, ulong opponentPositions);
        public List<BasePiece> SeparateUpdatePieceList(List<IObserver> opponentList, ulong occupiedPos);
    }
}

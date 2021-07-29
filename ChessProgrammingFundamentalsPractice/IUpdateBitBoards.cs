using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public interface IUpdateBitBoards
    {
        public void UpdateAllBitBoard(bool attacked, Player currentPlayer, Player opponent, ulong choosenPositionToMove, ulong opportunities, ulong currentPosition, ulong boardWithAllMember);
        public ulong[] SeparateUpdateBitBoardsToEvadeCheck(ulong newPositionOfDefender, ulong oldPositionOfDefender, ulong defenderPieceRoute, ulong allPiecePositions, ulong ourPositions, ulong opponentPositions);
        public List<IObserver> SeparateUpdatePieceList(List<IObserver> opponentList, ulong occupiedPos);
    }
}

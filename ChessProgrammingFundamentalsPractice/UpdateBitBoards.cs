using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class UpdateBitBoards
    {
        public void UpdateAllBitBoard(bool attacked, Player currentPlayer, Player opponent, ulong choosenPositionToMove, ulong opportunities, ulong currentPosition, ulong boardWithAllMember)
        {
            if (attacked)
            {
                opponent.NotifyBeingAttacked(choosenPositionToMove);
            }
            currentPlayer.NotifyMove(currentPosition, opportunities, choosenPositionToMove);
            boardWithAllMember = currentPlayer.Pieces ^ opponent.Pieces;
        }
    }
}

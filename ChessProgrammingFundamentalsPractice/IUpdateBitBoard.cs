using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public interface IUpdateBitBoard
    {
        void UpdateAllBitBoard(bool attacked, Player currentPlayer, Player opponent, ulong choosenPositionToMove, ulong opportunities, ulong currentPosition, ulong boardWithAllMember);
    }
}

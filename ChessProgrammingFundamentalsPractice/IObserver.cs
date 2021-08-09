using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public interface IObserver
    {
        public void UpdatePositionWhenMove(ulong currentPosition, ulong opportunities, ulong decidedMovePos);
        public void UpdatePositionWhenBeingAttacked();
    }
}

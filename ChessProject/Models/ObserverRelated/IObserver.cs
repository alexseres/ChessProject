using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.Models.ObserverRelated
{
    public interface IObserver
    {
        public void UpdatePositionWhenMove(ulong currentPosition, ulong opportunities, ulong decidedMovePos);
        public void UpdatePositionWhenBeingAttacked(ulong attackedPosition);
    }
}

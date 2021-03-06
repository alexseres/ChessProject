using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.Models.ObserverRelated
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void NotifyMove(ulong currentPosition, ulong opportunities, ulong decidedMovePos, bool weAttacked);
        void NotifyBeingAttacked(ulong pos);
    }
}

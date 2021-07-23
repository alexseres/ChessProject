using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void NotifyMove(ulong currentPosition, ulong opportunities, ulong decidedMovePos);
        void NotifyBeingAttacked(ulong pos);
    }
}

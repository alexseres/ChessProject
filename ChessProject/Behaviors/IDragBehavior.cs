using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.Behaviors
{
    public interface IDragBehavior
    {
        public void OnDrag(int col, int row);
    }
}

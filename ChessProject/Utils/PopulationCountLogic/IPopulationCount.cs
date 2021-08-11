using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.ActionLogics
{
    public interface IPopulationCount
    {
        public int GetPopulation(ulong bits);
    }
}

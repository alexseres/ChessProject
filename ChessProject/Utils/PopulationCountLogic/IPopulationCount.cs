using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.Utils.PopulationCountLogic
{
    public interface IPopulationCount
    {
        public int GetPopulation(ulong bits);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.ActionLogics.BitScanLogic
{
    public interface IBitScan
    {
        public int bitScanReverseMS1B(ulong bitBoard);
        public int bitScanForwardLS1B(ulong bitBoard);
    }
}

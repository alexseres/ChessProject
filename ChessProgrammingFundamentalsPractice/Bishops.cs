using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class Bishops : BasePiece
    {
        public ILongMovements Movements { get; set; }
        public IBitScan BitScan { get; set; }

        public Bishops(ColorSide color, ulong positions, IBitScan bitScan, ILongMovements movements) : base(color, positions)
        {
            Movements = movements;
            BitScan = bitScan;
        }
        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            throw new NotImplementedException();
        }
    }
}

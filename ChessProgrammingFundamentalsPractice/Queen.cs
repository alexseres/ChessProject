using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class Queen : BasePiece
    {
        public IBitScan BitScan { get; set; }
        public ILongMovements Movements { get; set; }
        public IRayAttack Attack { get; set; }
        private const int NorthDirection = 8;
        private const int EastDiretion = -1;
        private const int SouthDirection = -8;
        private const int WestDirection = 1;
        private const int EastNorthDirection = 7;
        private const int WestNorthDirection = 9;
        private const int EastSouthDirection = -9;
        private const int WestSouthDirection = -7;

        public Queen(ColorSide color, ulong positions, IBitScan bitScan, ILongMovements movements, IRayAttack attack) : base(color, positions)
        {
            BitScan = bitScan;
            Movements = movements;
            Attack = attack;

        }


        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            int square = BitScan.bitScanForwardLS1B(currentPosition);
            ulong northAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetNorth, BitScan.bitScanForwardLS1B, NorthDirection);
            ulong eastAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetEast, BitScan.bitScanReverseMS1B, EastDiretion);
            ulong southAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetSouth, BitScan.bitScanReverseMS1B, SouthDirection);
            ulong westAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetWest, BitScan.bitScanForwardLS1B, WestDirection);
            ulong eastNorthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetEastNorth, BitScan.bitScanForwardLS1B, EastNorthDirection);
            ulong westNorthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetWestNorth, BitScan.bitScanForwardLS1B, WestNorthDirection);
            ulong eastSouthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetEastSouth, BitScan.bitScanReverseMS1B, EastSouthDirection);
            ulong westSouthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetWestSouth, BitScan.bitScanReverseMS1B, WestSouthDirection);
            return northAttack ^ eastAttack ^ southAttack ^ westAttack ^ eastNorthAttack ^ westNorthAttack ^ eastSouthAttack ^ westSouthAttack;

        }
    }
}

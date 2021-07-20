﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class Bishops : BasePiece
    {
        public ILongMovements Movements { get; set; }
        public IBitScan BitScan { get; set; }
        public IRayAttack Attack { get; set; }

        private const int EastNorthDirection = 7;
        private const int WestNorthDirection = 9;
        private const int EastSouthDirection = -9;
        private const int WestSouthDirection = -7;


        public Bishops(ColorSide color, ulong positions, IBitScan bitScan, ILongMovements movements, IRayAttack attack) : base(color, positions)
        {
            Movements = movements;
            BitScan = bitScan;
            Attack = attack;
        }
        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            int square = BitScan.bitScanForwardLS1B(currentPosition);
            ulong eastNorthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetEastNorth, BitScan.bitScanForwardLS1B, EastNorthDirection);
            ulong westNorthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetWestNorth, BitScan.bitScanForwardLS1B, WestNorthDirection);
            ulong eastSouthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetEastSouth, BitScan.bitScanReverseMS1B, EastSouthDirection);
            ulong westSouthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetWestSouth, BitScan.bitScanReverseMS1B, WestSouthDirection);
            return eastNorthAttack ^ westNorthAttack ^ eastSouthAttack ^ westSouthAttack;
        }
    }
}

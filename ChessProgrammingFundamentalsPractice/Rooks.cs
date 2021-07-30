using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class Rooks : BasePiece
    {
        public IBitScan BitScan { get; set; }
        public ILongMovements Movements { get; set; }
        public IAttack Attack { get; set; }
        private const int NorthDirection = 8;
        private const int EastDiretion = -1;
        private const int SouthDirection = -8;
        private const int WestDirection = 1;

        public Rooks(ColorSide color, ulong positions, IBitScan bitScan, ILongMovements movements, IAttack attack) : base(color, positions)
        {
            BitScan = bitScan;
            Movements = movements;
            Attack = attack;
        }


        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            int square = BitScan.bitScanForwardLS1B(currentPosition);            
            ulong northAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetNorth,BitScan.bitScanForwardLS1B, NorthDirection);
            ulong eastAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetEast, BitScan.bitScanReverseMS1B, EastDiretion);
            ulong southAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetSouth, BitScan.bitScanReverseMS1B, SouthDirection);
            ulong westAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetWest, BitScan.bitScanForwardLS1B, WestDirection);
            Printboard(Convert.ToString((long)northAttack, toBase: 2).PadLeft(64, '0'));
            Console.WriteLine(" ");
            Printboard(Convert.ToString((long)eastAttack, toBase: 2).PadLeft(64, '0'));
            Console.WriteLine(" ");
            Printboard(Convert.ToString((long)southAttack, toBase: 2).PadLeft(64, '0'));
            Console.WriteLine(" ");
            Printboard(Convert.ToString((long)westAttack, toBase: 2).PadLeft(64, '0'));
            Console.WriteLine(" ");
            return northAttack ^ eastAttack ^ southAttack ^ westAttack;
        }

        public override ulong GetSpecificAttackFromSearch(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions, ulong opponentPiecePosition)
        {
            ulong[] allMoves = new ulong[8];
            int square = BitScan.bitScanForwardLS1B(currentPosition);
            ulong northAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetNorth, BitScan.bitScanForwardLS1B, NorthDirection);
            ulong eastAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetEast, BitScan.bitScanReverseMS1B, EastDiretion);
            ulong southAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetSouth, BitScan.bitScanReverseMS1B, SouthDirection);
            ulong westAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetWest, BitScan.bitScanForwardLS1B, WestDirection);
            allMoves[0] = northAttack;
            allMoves[1] = eastAttack;
            allMoves[2] = southAttack;
            allMoves[3] = westAttack;
            foreach (ulong moves in allMoves)
            {
                if ((moves & opponentPiecePosition) > 0)
                {
                    return moves;
                }
            }

            return 0;
        }

    }
}

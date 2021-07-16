using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class Rooks : BasePiece
    {
        public IBitScan BitScan { get; set; }
        public ILongMovements Movements { get; set; }
        private const int NorthDirection = 8;
        private const int EastDiretion = -1;
        private const int SouthDirection = -8;
        private const int WestDirection = 1;

        public Rooks(ColorSide color, ulong positions, IBitScan bitScan, ILongMovements movements) : base(color, positions)
        {
            BitScan = bitScan;
            Movements = movements;
        }


        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            int square = BitScan.bitScanForwardLS1B(currentPosition);
            ulong wnAttack = Movements.GetWestNorth(square);
            ulong enAttack = Movements.GetEastNorth(square);
            ulong northAttack = GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetNorth,BitScan.bitScanForwardLS1B, NorthDirection);
            ulong eastAttack = GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetEast, BitScan.bitScanReverseMS1B, EastDiretion);
            ulong southAttack = GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetSouth, BitScan.bitScanReverseMS1B, SouthDirection);
            ulong westAttack = GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetWest, BitScan.bitScanForwardLS1B, WestDirection);
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
        

        public ulong GetRayAttacks(ulong allPositionAtBoard, ulong opponent, int square, Func<int,ulong> rayAttack, Func<ulong,int> bitScan, int direction)
        {
            ulong attacks = rayAttack(square);
            ulong blocker = attacks & allPositionAtBoard;
            if (blocker > 0)
            {
                square = bitScan(blocker);
                if((opponent & ((ulong)1 <<square)) > 0)
                {
                    //this includes the actual line
                }
                else
                {
                    //so we add direction because in rayattacks it starts the count from the next line and excludes the current row
                    square += SetBitScanSubtracter(direction);

                }
                attacks = attacks ^ rayAttack(square) ;
            }
            return attacks;
        }

        public int SetBitScanSubtracter(int num)
        {
            switch (num)
            {
                case 8: //North
                    return -8;
                case 1: //West
                    return -1;
                case -8: //South
                    return 8;
                case -1: //East
                    return -1;
                default:
                    throw new Exception("wrong direction code");
            }
        }

        

      
    }
}

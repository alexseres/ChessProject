using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class Rooks : BasePiece
    {
        private const int NorthDirection = 8;
        private const int EastDiretion = 1;
        private const int SouthDirection = -8;
        private const int WestDirection = -1;

        public Rooks(ColorSide color, ulong positions) : base(color, positions)
        {
            
        }


        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            int square = bitScanForwardLS1B(currentPosition);
            ulong northAttack = GetPositiveRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, GetNorth, NorthDirection);
            ulong eastAttack = GetPositiveRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, GetEast, EastDiretion);
            ulong southAttack = GetNegativeRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, GetSouth, SouthDirection);
            ulong westAttack = GetNegativeRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, GetWest, WestDirection);
            Printboard(Convert.ToString((long)northAttack, toBase: 2).PadLeft(64, '0'));
            Console.WriteLine(" ");
            Printboard(Convert.ToString((long)eastAttack, toBase: 2).PadLeft(64, '0'));
            Console.WriteLine(" ");
            Printboard(Convert.ToString((long)southAttack, toBase: 2).PadLeft(64, '0'));
            Console.WriteLine(" ");
            Printboard(Convert.ToString((long)westAttack, toBase: 2).PadLeft(64, '0'));
            Console.WriteLine(" ");
            return northAttack ;
        }
        

        public ulong GetNegativeRayAttacks(ulong allPositionAtBoard, ulong opponent, int square, Func<int, ulong> rayAttack, int direction)
        {
            ulong attacks = rayAttack(square);
            //Printboard(Convert.ToString((long)attacks, toBase: 2).PadLeft(64, '0'));
            ulong blocker = attacks & allPositionAtBoard;
            if(blocker > 0)
            {
                square = bitScanReverseMS1B(blocker) + SetBitScanSubtracter(direction);
                attacks = (attacks & ~rayAttack(square)) ;
                
            }
            return attacks;
        }

        public ulong GetPositiveRayAttacks(ulong allPositionAtBoard, ulong opponent, int square, Func<int,ulong> rayAttack, int direction)
        {
            ulong attacks = rayAttack(square);
            //Printboard(Convert.ToString((long)attacks, toBase: 2).PadLeft(64, '0'));
            //Console.WriteLine(" ");
            ulong blocker = attacks & allPositionAtBoard;
            //Printboard(Convert.ToString((long)blocker, toBase: 2).PadLeft(64, '0'));
            //Console.WriteLine(" ");
            if (blocker > 0)
            {
                //so we subtract 8 because in rayattacks it starts the count from the next line and excludes the current row
                square = bitScanForwardLS1B(blocker) + SetBitScanSubtracter(direction);
                attacks = attacks & ~rayAttack(square);
            }
            //Printboard(Convert.ToString((long)attacks, toBase: 2).PadLeft(64, '0'));
            return attacks;
        }

        public int SetBitScanSubtracter(int num)
        {
            switch (num)
            {
                case 8: //North
                    return -8;
                case 1: //East
                    return -1;
                case -8: //South
                    return 8;
                case -1: //West
                    return 1;
                default:
                    throw new Exception("wrong direction code");
            }
        }

        public ulong GetNorth(int sq)
        {
            return (ulong)0x0101010101010100 << sq;
        }

        public ulong GetWest(int sq)
        {
            ulong one = 1;
            return 2 * ((one << (sq | 7)) - (one << sq));
        }

        public ulong GetSouth(int square)
        {
            return (ulong)0x0080808080808080 >> (square ^ 63);
        }

        public ulong GetEast(int square)
        {
            ulong one = 1;
            return (one << square) - (one << (square & 56));
        }

        public static int[] Index64 = new int[]
        {
            0, 47, 1, 56, 48,27, 2, 60,
            57, 49, 41, 37, 28, 16, 3, 61,
            54, 58, 35, 52, 50, 42, 21, 44,
            38, 32, 29, 23, 17, 11, 4, 62,
            46, 55, 26, 59, 40, 36, 15, 53,
            34, 51, 20, 43, 31, 22, 10, 45,
            25, 39, 14, 33, 19, 30, 9, 24,
            13, 18, 8, 12, 7, 6, 5, 63
        };

        public static ulong debruijn64 = 0x03f79d71b4cb0a89;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitBoard"></param>
        /// <returns>index (0..63) of least significant one bit</returns>
        public static int bitScanForwardLS1B(ulong bitBoard)
        {
            if (bitBoard == 0) throw new Exception("Bitboard cannot have 0 value");
            var result = Index64[((bitBoard ^ (bitBoard - 1)) * debruijn64) >> 58];
            return result;
        }

        public static int bitScanReverseMS1B(ulong bitBoard)
        {
            if (bitBoard == 0) Console.WriteLine("cannot be zero");
            bitBoard |= bitBoard >> 1;
            bitBoard |= bitBoard >> 2;
            bitBoard |= bitBoard >> 4;
            bitBoard |= bitBoard >> 8;
            bitBoard |= bitBoard >> 16;
            bitBoard |= bitBoard >> 32;
            int result = Index64[(bitBoard * debruijn64) >> 58];
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class RayAttack : IRayAttack
    {
        public ulong GetRayAttacks(ulong allPositionAtBoard, ulong opponent, int square, Func<int, ulong> rayAttack, Func<ulong, int> bitScan, int direction)
        {
            ulong attacks = rayAttack(square);
            Printboard(Convert.ToString((long)attacks, toBase: 2).PadLeft(64, '0'));

            ulong blocker = attacks & allPositionAtBoard;
            Printboard(Convert.ToString((long)blocker, toBase: 2).PadLeft(64, '0'));
            if (blocker > 0)
            {
                square = bitScan(blocker);
                if ((opponent & ((ulong)1 << square)) > 0)
                {
                    //this includes the actual line
                }
                else
                {
                    //so we add direction because in rayattacks it starts the count from the next line and excludes the current row
                    square += SetBitScanSubtracter(direction);

                }
                attacks = attacks ^ rayAttack(square);
            }
            return attacks;
        }

        public void Printboard(string board)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < board.Length; i++)
            {
                if (i % 8 == 0 && i != 0)
                {
                    string row = new string(sb.ToString());
                    Console.WriteLine(row);
                    sb.Clear();
                }
                sb.Append(board[i]);
            }
            var finalrow = new string(sb.ToString());
            Console.WriteLine(finalrow);

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
                case 9: //WestNorth
                    return -9;
                case 7: //EastNorth
                    return -7;
                case -7: //WestSouth
                    return 7;
                case -9: // EastSouth
                    return 9;
                    
                default:
                    throw new Exception("wrong direction code");
            }
        }

    }
}

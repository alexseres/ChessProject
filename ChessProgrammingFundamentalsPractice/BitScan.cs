using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class BitScan : IBitScan
    {
        public int[] Index64 = new int[]
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

        public ulong debruijn64 = 0x03f79d71b4cb0a89;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitBoard"></param>
        /// <returns>index (0..63) of least significant one bit</returns>
        public int bitScanForwardLS1B(ulong bitBoard)
        {
            if (bitBoard == 0) throw new Exception("Bitboard cannot have 0 value");
            var result = Index64[((bitBoard ^ (bitBoard - 1)) * debruijn64) >> 58];
            return result;
        }

        public int bitScanReverseMS1B(ulong bitBoard)
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

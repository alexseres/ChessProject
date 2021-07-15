using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace ChessProgrammingFundamentalsPractice
{
    public static class BitwiseOperatorsGeneral
    {
        static ulong k1 = 0x5555555555555555;
        static ulong k2 = 0x3333333333333333;
        static ulong k4 = 0x0f0f0f0f0f0f0f0f;
        static ulong kf = 0x0101010101010101;
        public static void CheckBitwiseComplementOperators()
        {

            uint a = 0b_0000_1111_0000_1111_0000_1111_0000_1100;
            Console.WriteLine(Convert.ToString(a, toBase: 2));
            uint b = ~a;
            Console.WriteLine(Convert.ToString(b, toBase: 2));
        }

        public static void CheckLeftShiftOperator()
        {
            uint x = 0b_1100_1001_0000_0000_0000_000_0001_0001;
            Console.WriteLine($"Before: {Convert.ToString(x, toBase: 2)}");

            uint y = x << 4;
            Console.WriteLine($"After: {Convert.ToString(y, toBase: 2)}");

            byte a = 0b_1111_0001;
            var b = a << 8;
            Console.WriteLine(a);  // convert it to int
            Console.WriteLine(Convert.ToString(a, toBase: 2));
            Console.WriteLine(b.GetType());
            Console.WriteLine($"Shifted byte: {Convert.ToString(b, toBase: 2)}");
        }

        public static void CheckRightShiftOperator()
        {
            uint x = 0b_1001;
            Console.WriteLine($"Before : {Convert.ToString(x, toBase: 2),4}");
            uint y = x >> 2;
            Console.WriteLine($"After: {Convert.ToString(y, toBase: 2),4}");
            int a = int.MinValue;
            Console.WriteLine(Convert.ToString(a, toBase: 2));
            int b = a >> 3;
            Console.WriteLine(Convert.ToString(b, toBase: 2));

            uint c = 0b_1000_0000_0000_0000_0000_0000_0000_0000;
            Console.WriteLine($"Before: {Convert.ToString(c, toBase: 2),32}");

            uint d = c >> 3;
            Console.WriteLine($"After : {Convert.ToString(d, toBase: 2)}");
        }

        public static void CheckLogicalAND_Operator()
        {
            uint a = 0b_1111_1000;
            uint b = 0b_1001_1101;
            uint c = a & b;
            Console.WriteLine(Convert.ToString(c, toBase: 2));

        }

        public static void CheckLogicalExclusiveOR_Operator()
        {
            uint a = 0b_0111_1000;
            uint b = 0b_0011_1100;
            uint c = a ^ b;
            Console.WriteLine(Convert.ToString(c, toBase: 2));
        }

        public static void CheckCompoundAsignment()
        {
            uint a = 0b_1111_1000;
            a &= 0b_1001_1101;
            Console.WriteLine(Convert.ToString(a, toBase: 2));

            byte x = 0b_1111_0001;
            int b = x << 8;
            Console.WriteLine(Convert.ToString(b, toBase: 2));
            x <<= 8;
            Console.WriteLine(x);
        }

        //list orders bitwise and shift operatins starting from the highest precedence to the lowest
        //-Bitwise Complement operator ~
        //-Shift operators << and >>
        //-Logical And perator & 
        //-Logical exclusive OR operator ^
        //-Logical OR operator | 
        public static void CalculateInionOfComplements()
        {
            byte a = 0b_1100;
            byte b = 0b_1001;
            Console.WriteLine($"a = {Convert.ToString(a, toBase: 2)}");
            Console.WriteLine($"~a = {Convert.ToString(~a, toBase: 2)}");
            Console.WriteLine($"b = {Convert.ToString(b, toBase: 2)}");
            Console.WriteLine($"~b = {Convert.ToString(~b, toBase: 2)}");
            Console.WriteLine($"a&~b = {Convert.ToString(a & ~b, toBase: 2)}");
            Console.WriteLine($"b&~a = {Convert.ToString(b & ~a, toBase: 2)}");
            Console.WriteLine($"a&~b | b&~a = {Convert.ToString((a & ~b) | (b & ~a), toBase: 2)}");
            Console.WriteLine($"-1 ^ b = {Convert.ToString((-1 ^ b), toBase: 2)}");
            Console.WriteLine(Convert.ToString(a ^ b, toBase: 2));
        }

        public static ulong RotateLeft(ulong x, int s)
        {
            int INT_BITS = 64;
            Console.WriteLine("Left Rotation");
            Console.WriteLine($"x = {x}");
            Console.WriteLine($"s = {s}");
            Console.WriteLine($"x(ulong) =  {x} to bits {Convert.ToString((byte)x, toBase: 2)} ");
            Console.WriteLine($"64 - s = {Convert.ToString(64 - s, toBase: 2)}");

            Console.WriteLine($"(x << s) = {Convert.ToString(((byte)x << s), toBase: 2)}");
            Console.WriteLine($"(x >> (64  - s)) = {Convert.ToString((byte)x >> (INT_BITS - s), toBase: 2)}");
            Console.WriteLine($"(x << s) | (x >> (INT_BITS - s) = {Convert.ToString(((byte)x << s) | ((byte)x >> (INT_BITS - s)), toBase: 2)}");
            ulong result = (x << s) | (x >> (INT_BITS - s));
            Console.WriteLine(result);
            return result;
        }

        public static ulong RotateRight(ulong x, int s)
        {
            int INT_BITS = 64;
            Console.WriteLine("Right Rotation");
            Console.WriteLine($"(x >> s) = {Convert.ToString(((byte)x >> s), toBase: 2)}");
            Console.WriteLine();
            Console.WriteLine($"(x << (64  - s)) = {Convert.ToString((byte)x << (INT_BITS - s), toBase: 2)}");
            Console.WriteLine((byte)x << (INT_BITS - s));
            Console.WriteLine($"(x >> s) | (x << (INT_BITS - s) = {Convert.ToString(((byte)x >> s) | ((byte)x << (INT_BITS - s)), toBase: 2)}");
            ulong result = (x >> s) | (x << (INT_BITS - s));
            Console.WriteLine(result);
            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">any bitboard</param>
        /// <param name="s">shift amount -64 < s < +64 left if positive right if negative</param>
        /// <returns>shifted board</returns>
        public static ulong GeneralizedShit(ulong x, int s)
        {

            char left = (char)s;
            char right = (char)(-((s >> 8) & left));
            ulong result = (x >> right) << (right + left);
            string binaryForm = Convert.ToString((long)result, 2).PadLeft(64,'0');
            int length = binaryForm.Length;
             return result;
        }

        public static ulong GeneralizedShift_ShorterVersion(ulong x, int s)
        {
            var result = (s > 0) ? (x << s) : (x >> -s);
            return result;
        }

        /// <summary>
        /// swap n none overlapping bits of bit index i with j
        /// </summary>
        /// <param name="b">any bitboard</param>
        /// <param name="i">position of bit sequence to swap</param>
        /// <param name="j">position of bit sequence to swap</param>
        /// <param name="n">n number of consecutive bits to swap</param>
        /// <returns>bitboard b with swapped bit sequences</returns>
        public static ulong SwapBits(ulong b, int i, int j, int n)
        {
            string printedBoard = Convert.ToString((long)b, toBase: 2).PadLeft(64, '0');
            SwapNeededNumber(printedBoard, i, j, n);
            //printBoardLikeCHess(printedBoard);
            
            Console.WriteLine(Convert.ToString((long)b, toBase: 2).PadLeft(64, '0'));
            ulong m = (ulong)((1 << n) - 1);
            string mStr = Convert.ToString((long)(ulong)((1 << n) - 1), toBase: 2).PadLeft(64,'0');
            //printBoardLikeCHess(mStr);
            Console.WriteLine(mStr);
            ulong x = ((b >> i) ^ (b >> j)) & m;
            var xStr = Convert.ToString((long)(((b >> i) ^ (b >> j)) & m), toBase: 2).PadLeft(64, '0');
            //printBoardLikeCHess(resultStr);
            //return result;
            return new ulong();
        }


        //POPCOUNT

        public static void CheckPopulationCount(ulong x)
        {
            if(x != 0 && (x & (x - 1)) == 0)
            {
                Console.WriteLine("Population count is one, power of two value");
            }
            else if((x &(x - 1)) == 0)
            {
                
                Console.WriteLine("Population count is less or equal than one");
            }
            else if((x &(x - 1)) > 1)
            {
                Console.WriteLine("Population count is bigger than one");
            }
        }

        public static byte[] InitPopulationCount()
        {
            byte[] populationCountOfByte256 = new byte[256];
            for(int i =1;i < 256; i++)
            {
               populationCountOfByte256[i] = (byte)(populationCountOfByte256[i / 2] + (i & 1));
            }
            return populationCountOfByte256;
        }

        

        public static ulong PopCountWith3Table(ulong x, ulong y, ulong z)
        {
            ulong maj = ((x ^ y) & z) | (x & y);
            ulong odd = ((x ^ y) ^ z);
            maj = maj - ((maj >> 1) & k1);
            odd = odd - ((odd >> 1) & k1);
            maj = (maj & k2) + ((maj >> 2) & k2);
            odd = (odd & k2) + ((odd >> 2) & k2);
            maj = (maj + (maj >> 4)) & k4;
            odd = (odd + (odd >> 4)) & k4;
            odd = ((maj + maj + odd) * kf) >> 56;
            return odd;
        }

        public static int popCount(byte[] popCountOfByte256,ulong x)
        {
            var result =  popCountOfByte256[x & 0xff] +
                   popCountOfByte256[(x >> 8) & 0xff] +
                   popCountOfByte256[(x >> 16) & 0xff] +
                   popCountOfByte256[(x >> 24) & 0xff] +
                   popCountOfByte256[(x >> 32) & 0xff] +
                   popCountOfByte256[(x >> 40) & 0xff] +
                   popCountOfByte256[(x >> 48) & 0xff] +
                   popCountOfByte256[x >> 56];
            return result; 
        }

        public static void SwapNeededNumber(string board, int posUpper, int posDown, int inBetween)
        {
            var first = 64 - posUpper;
            var second = 64 - posDown;
            for(int i = 0;i < board.Length; i++)
            {
                if (i >= first - inBetween && i <= first)
                {
                    Console.Write("5");
                }
                else if (i >= second - inBetween && i <= second)
                {
                    Console.Write("5");
                }
                else
                {
                    Console.Write(board[i]);
                }
            }
            Console.WriteLine();
        }

        public static void printBoardLikeCHess(string board)
        {
            //string reversedBoard = board.ToCharArray().Reverse().ToString();
            StringBuilder sb = new StringBuilder();
            for(int i = 0;i < board.Length;i++)
            {
                if (i % 8 == 0 && i != 0)
                {
                    string row = new string(sb.ToString().Reverse().ToArray());
                    Console.WriteLine(row);
                    sb.Clear();
                }
                sb.Append(board[i]);
            }
            var finalRow = new string(sb.ToString().Reverse().ToArray());
            Console.WriteLine(finalRow);
            Console.WriteLine(" ");
        }

        //BitScan

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
            Console.WriteLine(Convert.ToString((long)bitBoard, toBase:2).PadLeft(64, '0'));
            Console.WriteLine(Convert.ToString((long)debruijn64, toBase:2).PadLeft(64, '0'));
            if (bitBoard == 0) throw new Exception("Bitboard cannot have 0 value");
            var result =  Index64[((bitBoard ^ (bitBoard - 1)) * debruijn64) >> 58];
            Console.WriteLine(Convert.ToString(result, toBase:2).PadLeft(64, '0'));
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
            int result =  Index64[(bitBoard * debruijn64) >> 58];
            return result;
        }
        
        public static ulong flipVertical(ulong x)
        {
            const ulong k1 = 0x00FF00FF00FF00FF;
            const ulong k2 = 0x0000FFFF0000FFFF;
            printBoardLikeCHess(Convert.ToString((long)k1, toBase: 2).PadLeft(64, '0'));
            printBoardLikeCHess(Convert.ToString((long)k2,toBase: 2).PadLeft(64, '0'));
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            x = ((x >> 8) & k1) | ((x & k1) << 8);
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            x = ((x >> 16) & k2) | ((x & k2) << 16);
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            x = (x >> 32) | (x << 32);
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            return x;
        }

        public static ulong flipHorizontal(ulong x)
        {
            ulong k1 = 0x5555555555555555;
            ulong k2 = 0x3333333333333333;
            ulong k4 = 0x0f0f0f0f0f0f0f0f;
            printBoardLikeCHess(Convert.ToString((long)k1, toBase: 2).PadLeft(64, '0'));
            printBoardLikeCHess(Convert.ToString((long)k2, toBase: 2).PadLeft(64, '0'));
            printBoardLikeCHess(Convert.ToString((long)k4, toBase: 2).PadLeft(64, '0'));
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            x = ((x >> 1) & k1) | ((x & k1) << 1);
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            x = ((x >> 2) & k2) | ((x & k2) << 2);
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            x = ((x >> 4) & k4) | ((x & k4) << 4);
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            return x;
        }


        public static ulong FlipDiagonal(ulong x)
        {
            ulong t = 0;
            ulong k1 = 0x5500550055005500;
            ulong k2 = 0x3333000033330000;
            ulong k4 = 0x0f0f0f0f00000000;
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            t = k4 & (x ^ (x << 28));
            x ^= t ^ (t >> 28);
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            t = k2 & (x ^ (x << 14));
            x ^= t ^ (t >> 14);
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            t = k1 & (x ^ (x << 7));
            x ^= t ^ (t >> 7);
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            return x;
        }

        public static ulong FlipAntiDiagonal(ulong x)
        {
            ulong t = 0;
            ulong k1 = 0xaa00aa00aa00aa00;
            ulong k2 = 0xcccc0000cccc0000;
            ulong k4 = 0xf0f0f0f00f0f0f0f;
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            t = x ^ (x << 36);
            x ^= k4 & (t ^ (x >> 36));
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            t = k2 & (x ^ (x << 18));
            x ^=  (t ^ (t >> 18));
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            t = k1 & (x ^ (x << 9));
            x ^= t ^ (t >> 9);
            printBoardLikeCHess(Convert.ToString((long)x, toBase: 2).PadLeft(64, '0'));
            return x;
        }

    }
}
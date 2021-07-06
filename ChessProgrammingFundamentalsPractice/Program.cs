using System;

namespace ChessProgrammingFundamentalsPractice
{
    public class Program
    {
        static void Main(string[] args)
        {
            //BitwiseOperatorsGeneral.CheckBitwiseComplementOperators();
            //BitwiseOperatorsGeneral.CheckLeftShiftOperator();
            //BitwiseOperatorsGeneral.CheckRightShiftOperator();
            //BitwiseOperatorsGeneral.CheckLogicalAND_Operator();
            //BitwiseOperatorsGeneral.CheckLogicalExclusiveOR_Operator();
            //BitwiseOperatorsGeneral.CheckCompoundAsignment();
            //BitwiseOperatorsGeneral.CalculateInionOfComplements();
            //BitwiseOperatorsGeneral.RotateLeft(129, 2);
            //BitwiseOperatorsGeneral.RotateRight(129,2);

            ulong bitboard = 0b_0000_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111;
            ulong bitboard2= 0b_0000_0000_0000_0000_1110_1111_0000_0000_0110_0000_1110_0000_0000_0000_0000_0000;
            ulong bitboard3 = 0b_0000_0000_1100_0000_0010_0010_0000_0000_0000_0000_0000_0000_0000_0000_0011_0000;
            ulong bitboard4 = 0b_0000_0000_0000_0110_0000_0111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            //BitwiseOperatorsGeneral.GeneralizedShit(bitboard, 9);
            //BitwiseOperatorsGeneral.GeneralizedShift_ShorterVersion(bitboard, 9);
            //BitwiseOperatorsGeneral.SwapBits(bitboard2, 29, 41, 5);
            //BitwiseOperatorsGeneral.CheckPopulationCount(bitboard3);

            //byte[] byteArray = BitwiseOperatorsGeneral.InitPopulationCount();
            //int result = BitwiseOperatorsGeneral.popCount(byteArray, bitboard);
            //BitwiseOperatorsGeneral.PopCountWith3Table(bitboard, bitboard2, bitboard3);
            //BitwiseOperatorsGeneral.bitScanForwardLS1B(bitboard2);
            //BitwiseOperatorsGeneral.bitScanReverseMS1B(bitboard2);        
            //BitwiseOperatorsGeneral.flipVertical(bitboard3);
            //BitwiseOperatorsGeneral.flipHorizontal(bitboard3);
            //BitwiseOperatorsGeneral.FlipDiagonal(bitboard4);       
            BitwiseOperatorsGeneral.FlipAntiDiagonal(bitboard4); 
        }
    }
}

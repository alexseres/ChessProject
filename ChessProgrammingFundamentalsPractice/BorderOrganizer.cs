using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public static class BorderOrganizer
    {
        public static ulong OrganizeOrder(ColorSide color)
        {
            return color == ColorSide.Black ? 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_000_0000_0000_1111_1111 : 0b_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000; 
        }
    }
}

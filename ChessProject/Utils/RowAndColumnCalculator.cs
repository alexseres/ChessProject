using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ChessProject.Utils
{
    public static class RowAndColumnCalculator
    {

        public static List<(int,int)> GetPositionsOfRowsAndColumns(ulong moves)
        {
            ulong mask = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            List<(int, int)> Positions = new List<(int, int)>();
            for (int r = 0;r < 8;r++)
            {
                for(int c = 0;c < 8; c++)
                {
                    string str = Convert.ToString((long)moves, toBase: 2).PadLeft(64, '0');
                    string str2 = Convert.ToString((long)mask, toBase: 2).PadLeft(64, '0');
                    if ((mask & moves) > 0)
                    {
                        Positions.Add((c, r));
                    }
                    mask >>= 1;
                }
            }
            return Positions;
        }

        public static ulong UlongCalculator(int col, int row)
        {
            var move = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            for (int r = 0;r < 8; r++)
            {
                for(int c = 0;c < 8; c++)
                {
                    if(c == col && r == row)
                    {
                        return move;
                    }
                    move >>= 1;
                }
            }
            return 0;
        }

        public static (int, int) GetRowColumn(UniformGrid grid, Point position)
        {
            int column = -1;
            double total = 0;
            for(int c = 0;c < grid.Columns;c ++)
            {
                if(position.X < total)
                {
                    break;
                }
                column++;
                total += grid.ActualWidth / 8;
            }

            int row = -1;
            total = 0;
            for (int c = 0; c < grid.Columns; c++)
            {
                if(position.Y < total)
                {
                    break;
                }

                row++;
                total += grid.ActualHeight / 8;
            }
            return (column, row);
        }
    }
}

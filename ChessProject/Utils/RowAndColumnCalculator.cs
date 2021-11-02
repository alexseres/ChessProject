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

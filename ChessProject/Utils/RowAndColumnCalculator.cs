using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ChessProject.Utils
{
    public class RowAndColumnCalculator
    {
        public static (int, int) GetRowColumn(Grid grid, Point position)
        {
            int column = -1;
            double total = 0;
            foreach(ColumnDefinition clm in grid.ColumnDefinitions)
            {
                if(position.X < total)
                {
                    break;
                }
                column++;
                total += clm.ActualWidth;
            }

            int row = -1;
            total = 0;
            foreach(RowDefinition rowDef in grid.RowDefinitions)
            {
                if(position.Y < total)
                {
                    break;
                }

                row++;
                total += rowDef.ActualHeight;
            }
            return (column, row);
        }
    }
}

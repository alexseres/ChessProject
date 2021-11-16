using ChessProject.Models.ObserverRelated;
using ChessProject.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ChessProject.Utils
{
    public static class RowAndColumnCalculator
    {

        public static BasePiece GetBasePieceToRemoveFromObservableCollectionByMoves(List<IObserver> lst, ulong pos)
        {
            foreach(IObserver observer in lst)
            {
                BasePiece piece = observer as BasePiece;
                if (piece.Position == pos) return piece;
            }
            return null;
        }

        public static Dictionary<int, (int,int)> GetPositionsOfRowsAndColumns(ulong moves)
        {
            ulong mask = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            Dictionary<int, (int, int)> Positions = new Dictionary<int, (int, int)>();
            int counter = 0;
            for (int r = 0;r < 8;r++)
            {
                for(int c = 0;c < 8; c++)
                {
                    string str = Convert.ToString((long)moves, toBase: 2).PadLeft(64, '0');
                    string str2 = Convert.ToString((long)mask, toBase: 2).PadLeft(64, '0');
                    if ((mask & moves) > 0)
                    {
                        Positions[counter] = (c, r);
                    }
                    counter++;
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

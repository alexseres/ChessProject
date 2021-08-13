using ChessProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChessProject.Views
{
    /// <summary>
    /// Interaction logic for MainGameView.xaml
    /// </summary>
    public partial class MainGameView : Window
    {
        public MainGameViewModel _vm;
        public MainGameView()
        {
            InitializeComponent();
            _vm = new MainGameViewModel();
            this.DataContext = _vm;
            InitializeChessBoardGrid();
        }

        public void InitializeChessBoardGrid()
        {
           

            //we need this 2 object for setting the width and height of the grid cells because they could be just numbes, but they can also be things like auto so they can fit their contents
            //all these possibilities are encapsulated in GridLength object so we need to create one of them. we want to Grid cells to fit themselves to their contents we so specify Auto as the value
            GridLengthConverter gridLengthConverter = new GridLengthConverter();
            GridLength side = (GridLength)gridLengthConverter.ConvertFromString("Auto");
            for (int k = 0; k < 8; k++)
            {
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
                BoardGrid.ColumnDefinitions[k].Width = side;
                BoardGrid.RowDefinitions.Add(new RowDefinition());
                BoardGrid.RowDefinitions[k].Height = side;
            }
            Rectangle[,] square = new Rectangle[8, 8];

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    square[row, col] = new Rectangle();
                    square[row, col].Height = 50;
                    square[row, col].Width = 50;
                    Grid.SetColumn(square[row, col], col);
                    Grid.SetRow(square[row, col], row);
                    if ((row + col) % 2 == 0)
                    {
                        square[row, col].Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    }
                    else
                    {
                        square[row, col].Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    }
                    BoardGrid.Children.Add(square[row, col]);
                }
            }
        }
    }
}

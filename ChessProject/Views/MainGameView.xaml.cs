using ChessProject.ViewModels;
using System.Windows;
using System.Windows.Controls;
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
            _vm.BoardUniformGrid = BoardGrid;
        }

        public void InitializeChessBoardGrid()
        {
            Rectangle[,] square = new Rectangle[8, 8];
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    square[row, col] = new Rectangle();
                    square[row, col].Height = 60;
                    square[row, col].Width = 60;
                    Grid.SetColumn(square[row, col], col);
                    Grid.SetRow(square[row, col], row);
                    if ((row + col) % 2 == 0)
                    {
                        square[row, col].Fill = Brushes.SaddleBrown;
                    }
                    else
                    {
                        square[row, col].Fill = Brushes.PaleGoldenrod;
                    }
                    BoardGrid.Children.Add(square[row, col]);
                }
            }
        }
    }
}

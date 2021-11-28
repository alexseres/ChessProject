using ChessProject.ActionLogics;
using ChessProject.ActionLogics.Attacks;
using ChessProject.ActionLogics.BitBoardsUpdater;
using ChessProject.Actions.Movements;
using ChessProject.Models;
using ChessProject.Models.Enums;
using ChessProject.Models.ObserverRelated;
using ChessProject.Models.Pieces;
using ChessProject.Utils.CloneCollections;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Timers;
using System.Threading.Tasks;
using ChessProject.Commands;
using ChessProject.ServiceLayers;
using ChessProject.Utils.BitScanLogic;
using ChessProject.Utils.PopulationCountLogic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace ChessProject.ViewModels
{
    public class MainGameViewModel : BaseViewModel, IDropTarget
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public ObservableCollection<BasePiece> _pieceCollection;
        public ObservableCollection<BasePiece> PieceCollection { get { return _pieceCollection; } set { SetProperty(ref _pieceCollection, value); } }
        public IAttack Attack { get; set; }
        public IUpdateBitBoards UpdateBitBoards { get; set; }
        public IBitScan Scan { get; set; }
        public ILongMovements Movements { get; set; }
        public IPopulationCount PopCount { get; set; }  
        public UniformGrid BoardUniformGrid { get; set; }

        private Dictionary<int, Rectangle> CellsWherePlayerHasOpportunities { get; set; } = new Dictionary<int, Rectangle>();

        private RelayCommand<BasePiece> _pawnSwapperForPlayer1Command;
        public RelayCommand<BasePiece> PawnSwapperForPlayer1Command { get { return _pawnSwapperForPlayer1Command; } set { SetProperty(ref _pawnSwapperForPlayer1Command, value); } }

        private RelayCommand<BasePiece> _pawnSwapperForPlayer2Command;
        public RelayCommand<BasePiece> PawnSwapperForPlayer2Command { get { return _pawnSwapperForPlayer2Command; } set { SetProperty(ref _pawnSwapperForPlayer2Command, value); } }

        public SolidColorBrush _knockedPiecesBrushOfPlayer1;
        public SolidColorBrush KnockedPiecesBrushOfPlayer1 { get { return _knockedPiecesBrushOfPlayer1; } set { SetProperty(ref _knockedPiecesBrushOfPlayer1, value); } }
        public SolidColorBrush _knockedPiecesBrushOfPlayer2;
        public SolidColorBrush KnockedPiecesBrushOfPlayer2 { get { return _knockedPiecesBrushOfPlayer2; } set { SetProperty(ref _knockedPiecesBrushOfPlayer2, value); } }

        public string _exceptionMessage;
        public string ExceptionMessage { get { return _exceptionMessage; } set { SetProperty(ref _exceptionMessage, value); } }

        IMainGameService Servicer { get; set; }
        private bool DragOn { get; set; } = false;

        public MainGameViewModel()
        {
            
            PieceCollection = new ObservableCollection<BasePiece>();
            Player1 = new Player(PlayerType.Player1,ColorSide.Black);
            Player2 = new Player(PlayerType.Player2,ColorSide.White);
            PawnSwapperForPlayer1Command = new RelayCommand<BasePiece>(PawnSwapperForPlayer1, PawnSwapperForPlayer1CanExecute);
            PawnSwapperForPlayer2Command = new RelayCommand<BasePiece>(PawnSwapperForPlayer2, PawnSwapperForPlayer2CanExecute);
            KnockedPiecesBrushOfPlayer1 = Brushes.Brown;
            KnockedPiecesBrushOfPlayer2 = Brushes.Brown;
            ExceptionMessage = "";
            UpdateBitBoards = new UpdateBitBoards();
            Scan = new BitScan();
            Movements = new LongMovements();
            PopCount = new PopulationCount();
            Attack = new Attack(Scan, PopCount, UpdateBitBoards);
            Servicer = new MainGameService(Player1, Player2,PieceCollection, UpdateBitBoards, Scan, Movements, PopCount, Attack);

        }

        public bool PawnSwapperForPlayer1CanExecute(object obj)
        {
            return Player1.IsWaitedForPawnToBeSwappedToAnotherPiece == true ? true:false;
        }

        public void PawnSwapperForPlayer1(object obj)
        {
            BasePiece piece = obj as BasePiece;
            if (piece is null) return;
            Servicer.PawnSwapperLogicForPlayer1(piece);
            KnockedPiecesBrushOfPlayer1 = Brushes.Brown;
        }


        public bool PawnSwapperForPlayer2CanExecute(object obj)
        {
            return Player2.IsWaitedForPawnToBeSwappedToAnotherPiece == true ? true : false;
        }

        public void PawnSwapperForPlayer2(object obj)
        {
            BasePiece piece = obj as BasePiece;
            if (piece is null) return;
            Servicer.PawnSwapperLogicForPlayer2(piece);
            KnockedPiecesBrushOfPlayer2 = Brushes.Brown;
        }


        public void ColoringCellsAsOpportunitiesOfPiece(Dictionary<int, (int, int)> positions)
        {
            for(int i =0;i < BoardUniformGrid.Children.Count;i++)
            {
                if (positions.ContainsKey(i))
                {
                    CellsWherePlayerHasOpportunities[i] = BoardUniformGrid.Children[i] as Rectangle;
                    (BoardUniformGrid.Children[i] as Rectangle).Fill = Brushes.Lime;
                }
            }
        }

        public void MakeCellsOfOpportunitiesDisappear()
        {
            int counter = 0;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (CellsWherePlayerHasOpportunities.ContainsKey(counter))
                    {
                        if ((row + col) % 2 == 0)
                        {
                            (BoardUniformGrid.Children[counter] as Rectangle).Fill = Brushes.SaddleBrown;
                        }
                        else
                        {
                            (BoardUniformGrid.Children[counter] as Rectangle).Fill = Brushes.PaleGoldenrod;
                        }
                    }
                    counter++;
                }
            }
            CellsWherePlayerHasOpportunities.Clear();
        }
        public async void ExceptionMessageRemover()
        {
            await Task.Delay(2000);
            ExceptionMessage = "";
        }


        private async Task SwitchTimerOn()
        {
            DragOn = true;
            await Task.Delay(2000);
            MakeCellsOfOpportunitiesDisappear();
            DragOn = false;
        }
        public async void DragOver(IDropInfo dropInfo)
        {
            if (Player1.IsWaitedForPawnToBeSwappedToAnotherPiece== true || Player2.IsWaitedForPawnToBeSwappedToAnotherPiece == true) return;
            BasePiece piece = dropInfo.Data as BasePiece;
            if (piece is null) return;
            if(Servicer.NextPlayer != piece.Creator)
            {
                ExceptionMessage = "It is not your move";ExceptionMessageRemover();
                return;
            }
            if (!DragOn)
            {
                (bool permission,string message) = Servicer.ProcessOfMakingSurePlayerCanChooseSpecificPiece(piece.Creator, piece);
                if (!permission)
                {
                    ExceptionMessage = message;ExceptionMessageRemover();
                    return;
                }
                ColoringCellsAsOpportunitiesOfPiece(piece.Creator.PositionsOfOpportunities);
                await SwitchTimerOn();
            }
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Copy;

        }

        public async void WinnerMaker(Player winner)
        {
            await Task.Delay(2000);
            ExceptionMessage = $"The Winner is {winner.Color} Player. Congratulations.";
            await Task.Delay(200000);
            ExceptionMessage = "";
            foreach(Window item in Application.Current.Windows)
            {
                if (item.DataContext == this) item.Close();
            }
            Application.Current.MainWindow.Show();
        }

        
        public void Drop(IDropInfo dropInfo)
        {
            Point point = new Point { X = dropInfo.DropPosition.X, Y = dropInfo.DropPosition.Y };
            (int col, int row) = Utils.RowAndColumnCalculator.GetRowColumn(BoardUniformGrid, point);
            BasePiece piece = dropInfo.Data as BasePiece;
            Player player = piece.Creator;
            ulong move = Utils.RowAndColumnCalculator.UlongCalculator(col, row);
            MakeCellsOfOpportunitiesDisappear();
            (bool changeHappened,string message) = Servicer.ProcessOfMakingSurePlayerCanDropSpecificPiece(player, piece,piece.Position,player.RecentOpportunities, move);
            if (changeHappened) 
            {
                ExceptionMessage = message;ExceptionMessageRemover();
                if (player.IsWaitedForPawnToBeSwappedToAnotherPiece)
                {
                    if (player.PlayerNum == PlayerType.Player1) KnockedPiecesBrushOfPlayer1 = Brushes.Green; else KnockedPiecesBrushOfPlayer2 = Brushes.Green;
                }
                Servicer.CheckIfIsThreeFoldOrFiftyMove(player);
                CollectionViewSource.GetDefaultView(PieceCollection).Refresh();
                if(Player1.HasWon == true || Player2.HasWon == true)
                {
                    Player winner = Player1.HasWon == true ? Player1 : Player2;
                    WinnerMaker(winner);
                }
                
            }
            else
            {
                ExceptionMessage = message;ExceptionMessageRemover();
            }
        }
    }
}

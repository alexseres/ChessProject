using ChessProject.ActionLogics;
using ChessProject.ActionLogics.Attacks;
using ChessProject.ActionLogics.BitBoardsUpdater;
using ChessProject.ActionLogics.BitScanLogic;
using ChessProject.ActionLogics.PopulationCountLogic;
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

namespace ChessProject.ViewModels
{
    public class MainGameViewModel : BaseViewModel, IDropTarget
    {
        public Rook TestRook { get; set; }

        public ObservableCollection<BasePiece> _pieceCollection;
        public ObservableCollection<BasePiece> PieceCollection { get { return _pieceCollection; } set { SetProperty(ref _pieceCollection, value); } }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public const string From = "Choose a piece";
        public const string To = "Move with the piece to";
        public IAttack Attack { get; set; }
        public IUpdateBitBoards UpdateBitBoards { get; set; }
        public IBitScan Scan { get; set; }
        public ILongMovements Movements { get; set; }
        public IPopulationCount PopCount { get; set; }
        public ulong BoardWithAllMember = 0b_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111;
        
        public UniformGrid BoardUniformGrid { get; set; }
        private Player NextPlayer { get; set; }

        private Dictionary<int, Rectangle> CellsWherePlayerHasOpportunities { get; set; } = new Dictionary<int, Rectangle>();
        private bool DragOn { get; set; } = false;

        private bool IsWaitedForPawnToBeSwappedToAnotherPieceForPlayer1 { get; set; } = false;
        private bool IsWaitedForPawnToBeSwappedToAnotherPieceForPlayer2 { get; set; } = false;

        private RelayCommand<BasePiece> _pawnSwapperForPlayer1Command;
        public RelayCommand<BasePiece> PawnSwapperForPlayer1Command { get { return _pawnSwapperForPlayer1Command; } set { SetProperty(ref _pawnSwapperForPlayer1Command, value); } }

        private RelayCommand<BasePiece> _pawnSwapperForPlayer2Command;
        public RelayCommand<BasePiece> PawnSwapperForPlayer2Command { get { return _pawnSwapperForPlayer2Command; } set { SetProperty(ref _pawnSwapperForPlayer2Command, value); } }

        public SolidColorBrush _knockedPiecesBrushOfPlayer1;
        public SolidColorBrush KnockedPiecesBrushOfPlayer1 { get { return _knockedPiecesBrushOfPlayer1; } set { SetProperty(ref _knockedPiecesBrushOfPlayer1, value); } }
        public SolidColorBrush _knockedPiecesBrushOfPlayer2;
        public SolidColorBrush KnockedPiecesBrushOfPlayer2 { get { return _knockedPiecesBrushOfPlayer2; } set { SetProperty(ref _knockedPiecesBrushOfPlayer2, value); } }

        public String _exceptionMessage;
        public String ExceptionMessage { get { return _exceptionMessage; } set { SetProperty(ref _exceptionMessage, value); } }


        public MainGameViewModel()
        {
            PieceCollection = new ObservableCollection<BasePiece>();
            PawnSwapperForPlayer1Command = new RelayCommand<BasePiece>(PawnSwapperForPlayer1, PawnSwapperForPlayer1CanExecute);
            PawnSwapperForPlayer2Command = new RelayCommand<BasePiece>(PawnSwapperForPlayer2, PawnSwapperForPlayer2CanExecute);
            KnockedPiecesBrushOfPlayer1 = Brushes.Brown;
            KnockedPiecesBrushOfPlayer2 = Brushes.Brown;
            ExceptionMessage = "";
            //ColorSide color = SelectColorSide();
            ColorSide color = ColorSide.Black;
            UpdateBitBoards = new UpdateBitBoards();
            Scan = new BitScan();
            Movements = new LongMovements();
            PopCount = new PopulationCount();
            Attack = new Attack(Scan, PopCount, UpdateBitBoards);
            InitAllPieces(color, Scan, Movements, Attack);
            SelectPlayerWhoStarts(Player1, Player2);
        }

        public bool PawnSwapperForPlayer1CanExecute(object obj)
        {
            return IsWaitedForPawnToBeSwappedToAnotherPieceForPlayer1 == true ? true:false;
        }

        public void PawnSwapperForPlayer1(object obj)
        {
            BasePiece piece = obj as BasePiece;
            if (piece is null) return;
            PieceCollection.Remove(Player1.PawnToBeSwapped);
            Player1.SwapPawnToAnotherPiece(piece);
            PieceCollection.Add(piece);
            IsWaitedForPawnToBeSwappedToAnotherPieceForPlayer1 = false;
            KnockedPiecesBrushOfPlayer1 = Brushes.Brown;
        }

        public bool PawnSwapperForPlayer2CanExecute(object obj)
        {
            return IsWaitedForPawnToBeSwappedToAnotherPieceForPlayer2 == true ? true : false;
        }

        public void PawnSwapperForPlayer2(object obj)
        {
            BasePiece piece = obj as BasePiece;
            if (piece is null) return;
            PieceCollection.Remove(Player2.PawnToBeSwapped);
            Player2.SwapPawnToAnotherPiece(piece);
            PieceCollection.Add(piece);
            IsWaitedForPawnToBeSwappedToAnotherPieceForPlayer2 = false;
            KnockedPiecesBrushOfPlayer2 = Brushes.Brown;
        }

        public void SelectPlayerWhoStarts(Player player1, Player player2)
        {
            if(player1.Color == ColorSide.White)
            {
                NextPlayer = player1;
            }
            else
            {
                NextPlayer = player2;
            }
        }
       
 
        public ColorSide SelectColorSide()
        {
            Console.WriteLine("Which color you want it to be up? 'white' or 'black'");
            string answer = Console.ReadLine();
            if (answer == "white")
            {
                return ColorSide.White;
            }
            else
            {
                return ColorSide.Black;
            }
        }
        public void InitAllPieces(ColorSide choosenColorToBeUp, IBitScan bitScan, ILongMovements movements, IAttack attack)
        {
            ColorSide otherColor = choosenColorToBeUp == ColorSide.White ? ColorSide.Black : ColorSide.White;
            Player1 = new Player(choosenColorToBeUp);
            Player2 = new Player(otherColor);


            //pawn properties
            ulong doubleMoveSignForUp = 0b_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            ulong doubleMoveSignForDown = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000;
            ulong fifthLineOfEnPassantForUp = 0b_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000;
            ulong fifthLineOfEnPassantForDown = 0b_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000;
            ulong lastLineForSwapForUp = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_000_0000_0000_1111_1111;
            ulong lastLineForSwapToDown = 0b_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;

            #region InitIteratePieces
            ulong mask = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            for (int i = 0; i < 64; i++)
            {
                string imagePath = "";
                if (i == 0 || i == 7)
                {
                    imagePath = $"../ChessPiecePictures/{choosenColorToBeUp}Rook.png";
                    Rook rookUp = new Rook(Player1, choosenColorToBeUp, mask, bitScan, movements, attack, imagePath);
                    Player1.PiecesList.Add(rookUp);
                    Player1.PiecesPosition ^= mask;
                    PieceCollection.Add(rookUp);
                }
                else if (i == 1 || i == 6 )
                {
                    imagePath = $"../ChessPiecePictures/{choosenColorToBeUp}Knight.png";
                    Knight knightUp = new Knight(Player1, choosenColorToBeUp, mask, imagePath);
                    Player1.PiecesList.Add(knightUp);
                    Player1.PiecesPosition ^= mask;
                    PieceCollection.Add(knightUp);

                }
                else if (i == 2 || i == 5)
                {
                    imagePath = $"../ChessPiecePictures/{choosenColorToBeUp}Bishop.png";
                    Bishop bishopUp = new Bishop(Player1, choosenColorToBeUp, mask, bitScan, movements, attack, imagePath);
                    Player1.PiecesList.Add(bishopUp);
                    Player1.PiecesPosition ^= mask;
                    PieceCollection.Add(bishopUp);
                }
                else if (i == 3)
                {
                    imagePath = $"../ChessPiecePictures/{choosenColorToBeUp}King.png";
                    King kingUp = new King(Player1, choosenColorToBeUp, mask, imagePath);
                    Player1.PiecesList.Add(kingUp);
                    Player1.King = kingUp;
                    Player1.PiecesPosition ^= mask;
                    PieceCollection.Add(kingUp);
                }
                else if (i == 4)
                {
                    imagePath = $"../ChessPiecePictures/{choosenColorToBeUp}Queen.png";
                    Queen queenUp = new Queen(Player1, choosenColorToBeUp, mask, bitScan, movements, attack, imagePath);
                    Player1.PiecesList.Add(queenUp);
                    Player1.PiecesPosition ^= mask;
                    PieceCollection.Add(queenUp);
                }
                else if (i >= 8 && i <= 15)
                {
                    imagePath = $"../ChessPiecePictures/{choosenColorToBeUp}Pawn.png";
                    Pawn pawnUp = new Pawn(Player1, choosenColorToBeUp, mask, imagePath, lastLineForSwapForUp, doubleMoveSignForUp, fifthLineOfEnPassantForUp);
                    Player1.PiecesList.Add(pawnUp);
                    Player1.PiecesPosition ^= mask;
                    Player2.OpponentPawnsList.Add(pawnUp);
                    PieceCollection.Add(pawnUp);
                }
                else if (i > 47 && i < 56)
                {
                    imagePath = $"../ChessPiecePictures/{otherColor}Pawn.png";
                    Pawn pawnDown = new Pawn(Player2, otherColor, mask, imagePath, lastLineForSwapToDown, doubleMoveSignForDown, fifthLineOfEnPassantForDown);
                    Player2.PiecesList.Add(pawnDown);
                    Player2.PiecesPosition ^= mask;
                    Player1.OpponentPawnsList.Add(pawnDown);
                    PieceCollection.Add(pawnDown);
                }
                else if (i == 56 || i == 63)
                {
                    imagePath = $"../ChessPiecePictures/{otherColor}Rook.png";
                    Rook rookDown = new Rook(Player2, otherColor, mask, bitScan, movements, attack, imagePath);
                    Player2.PiecesList.Add(rookDown);
                    Player2.PiecesPosition ^= mask;
                    PieceCollection.Add(rookDown);
                }
                else if (i == 57 || i == 61)
                {
                    imagePath = $"../ChessPiecePictures/{otherColor}Bishop.png";
                    Bishop bishopDown = new Bishop(Player2, otherColor, mask, bitScan, movements, attack, imagePath);
                    Player2.PiecesList.Add(bishopDown);
                    Player2.PiecesPosition ^= mask;
                    PieceCollection.Add(bishopDown);
                }
                else if (i == 58 || i == 62)
                {
                    imagePath = $"../ChessPiecePictures/{otherColor}Knight.png";
                    Knight knightDown = new Knight(Player2, otherColor, mask, imagePath);
                    Player2.PiecesList.Add(knightDown);
                    Player2.PiecesPosition ^= mask;
                    PieceCollection.Add(knightDown);
                }
                else if (i == 59)
                {
                    imagePath = $"../ChessPiecePictures/{otherColor}Queen.png";
                    Queen queenDown = new Queen(Player2, otherColor, mask, bitScan, movements, attack, imagePath);
                    Player2.PiecesList.Add(queenDown);
                    Player2.PiecesPosition ^= mask;
                    PieceCollection.Add(queenDown);
                }
                else if (i == 60)
                {
                    imagePath = $"../ChessPiecePictures/{otherColor}King.png";
                    King kingDown = new King(Player2, otherColor, mask, imagePath);
                    Player2.PiecesList.Add(kingDown);
                    Player2.PiecesPosition ^= mask;
                    Player2.King = kingDown;
                    PieceCollection.Add(kingDown);

                }

                mask = mask >> 1;
            }

            #endregion

            Player2.King.OpponentKing = Player1.King;
            Player1.King.OpponentKing = Player2.King;
            Player2.OpponentPiecesList = Player1.PiecesList;
            Player1.OpponentPiecesList = Player2.PiecesList;
        }

        public bool CheckIfPlayer1IsWhite()
        {
            return Player1.Color == ColorSide.White ? true : false;
        }

        public bool ProcessOfMakingSurePlayerCanChooseSpecificPiece(Player player, BasePiece piece)
        {
            Player opponent = OpponentCreater(player);
            if (!(ColorCheck(player, opponent)))
            {
                NextPlayer = player;
                return false;
            }
            IsPlayerInCheckAndCheckmateChecker(player,opponent);
            player.RecentOpportunities = piece.Search(piece.Position, BoardWithAllMember, opponent.PiecesPosition, player.PiecesPosition);
            if (player.RecentOpportunities <= 0)
            {
                Console.WriteLine("you cannot move with this piece, choose another one");
                player.RecentOpportunities = 0;
                NextPlayer = player;
                return false;
            }
            player.PositionsOfOpportunities = Utils.RowAndColumnCalculator.GetPositionsOfRowsAndColumns(piece.Creator.RecentOpportunities);
            
            if (player.IsThreeFold == true || opponent.IsThreeFold == true)
            {
                Console.WriteLine("Its a draw because of TreeFold");   
            }
            if (player.IsFiftyMoveWIthoutCaptureOrPawnMove == true || opponent.IsFiftyMoveWIthoutCaptureOrPawnMove == true)
            {
                Console.WriteLine("Its draw because of 50 move rule");
            }

            return true;
            
        }

        public void IsPlayerInCheckAndCheckmateChecker(Player actualPlayer, Player opponent)
        {
            if (!actualPlayer.PlayerInCheck)
            {
                ulong opponentAttacks = Attack.GetAllOpponentAttackToCheckIfKingInCheck(actualPlayer.King.Position, BoardWithAllMember, opponent.PiecesPosition, actualPlayer.PiecesPosition, opponent.PiecesList);
                if (opponentAttacks > 0)   // king in check
                {
                    Console.WriteLine($"Check for {actualPlayer.Color} Player");
                    actualPlayer.PlayerInCheck = true;
                    if (Attack.GetCounterAttackToChekIfSomePieceCouldEvadeAttack(opponentAttacks, actualPlayer.King.Position, BoardWithAllMember, opponent.PiecesPosition, actualPlayer.PiecesPosition, actualPlayer.PiecesList, opponent.PiecesList))
                    {
                        Console.WriteLine("CheckMATE");
                        //break;
                    }
                }
            }
        }

        public bool ColorCheck(Player actualPlayer, Player opponent)
        {
            if(actualPlayer.Color == opponent.Color)
            {
                Console.WriteLine($"Choose piece is not {actualPlayer.Color} piece");
                return false;
            }
            return true;   
        }

        public bool ProcessOfMakingSurePlayerCanDropSpecificPiece(Player player, BasePiece piece, ulong currentPiecePosition, ulong opportunities, ulong choosenPositionToMove)
        {
            Player opponent = OpponentCreater(player);
            if ((choosenPositionToMove & opportunities) <= 0)
            {
                Console.WriteLine("You cannot move there because there is no opportunity there");
                player.RecentOpportunities = 0;
                player.PositionsOfOpportunities.Clear();
                NextPlayer = player;
                return false;
            }

            bool attacked = Attack.HasAttacked(choosenPositionToMove, opponent.PiecesPosition);

            if (!CheckProcess(player, currentPiecePosition, opponent, piece, attacked, choosenPositionToMove))
            {
                NextPlayer = player;
                player.RecentOpportunities = 0;
                player.PositionsOfOpportunities.Clear();
                return false;
            }
            if(player.CheckIfCurrentAtLastLineAndIsPawn(choosenPositionToMove, piece) && player.KnockedPieces.Count != 0)
            {
                if(player == Player1)
                {
                    IsWaitedForPawnToBeSwappedToAnotherPieceForPlayer1 = true;
                    KnockedPiecesBrushOfPlayer1 = Brushes.Green;
                    ExceptionMessage = $"Choose piece {player.Color} player";
                }
                else
                {
                    IsWaitedForPawnToBeSwappedToAnotherPieceForPlayer2 = true;
                    KnockedPiecesBrushOfPlayer2 = Brushes.Green;
                    ExceptionMessage = $"Choose piece {player.Color} player";
                }
            }

            RemoveFromPieceCollectionBecauseAttacked(attacked, opponent, choosenPositionToMove);
            UpdateBitBoards.UpdateAllBitBoard(attacked, player, opponent, choosenPositionToMove, opportunities, currentPiecePosition, ref BoardWithAllMember);
            player.PositionsOfOpportunities.Clear();
            NextPlayer = opponent;
            return true;
        }

        public void RemoveFromPieceCollectionBecauseAttacked(bool isAttacked, Player opponent, ulong choosenPositionToMove)
        {
            if (isAttacked)
            {
                BasePiece pieceToBeRemoved = Utils.RowAndColumnCalculator.GetBasePieceToRemoveFromObservableCollectionByMoves(opponent.PiecesList, choosenPositionToMove);
                if (!(pieceToBeRemoved is null))
                {
                    PieceCollection.Remove(pieceToBeRemoved);
                }
            }
        }

        public Player OpponentCreater(Player player)
        {
            if (player == Player1)
            {
                return Player2;
            }
            else
            {
                return Player1;
            }
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

        private async Task SwitchTimerOn()
        {
            DragOn = true;
            await Task.Delay(2000);
            MakeCellsOfOpportunitiesDisappear();
            DragOn = false;
        }


        public async void DragOver(IDropInfo dropInfo)
        {
            if (IsWaitedForPawnToBeSwappedToAnotherPieceForPlayer1 == true  || IsWaitedForPawnToBeSwappedToAnotherPieceForPlayer2 == true) return;
            if (!DragOn)
            {
                BasePiece piece = dropInfo.Data as BasePiece;
                if (piece is null) return;
                if (NextPlayer.Color != piece.Creator.Color) return;
                bool pieceCanGo = ProcessOfMakingSurePlayerCanChooseSpecificPiece(piece.Creator, piece);
                if (!pieceCanGo) return;
                ColoringCellsAsOpportunitiesOfPiece(piece.Creator.PositionsOfOpportunities);
                await SwitchTimerOn();
            }
            
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Copy;
        }

        
        public void Drop(IDropInfo dropInfo)
        {
            Point point = new Point { X = dropInfo.DropPosition.X, Y = dropInfo.DropPosition.Y };
            (int col, int row) = Utils.RowAndColumnCalculator.GetRowColumn(BoardUniformGrid, point);
            BasePiece piece = dropInfo.Data as BasePiece;
            Player player = piece.Creator;
            ulong move = Utils.RowAndColumnCalculator.UlongCalculator(col, row);
            MakeCellsOfOpportunitiesDisappear();
            bool changeHappened = ProcessOfMakingSurePlayerCanDropSpecificPiece(player, piece,piece.Position,player.RecentOpportunities, move);
            if(changeHappened)CollectionViewSource.GetDefaultView(PieceCollection).Refresh();
        }

        public void Printboard(string board)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < board.Length; i++)
            {
                if (i % 8 == 0 && i != 0)
                {
                    string row = new string(sb.ToString());
                    Console.WriteLine(row);
                    sb.Clear();
                }
                sb.Append(board[i]);
            }
            var finalrow = new string(sb.ToString());
            Console.WriteLine(finalrow);

        }


        public bool CheckProcess(Player player, ulong currentPiecePosition, Player opponent, BasePiece piece, bool attacked, ulong choosenPositionToMove)
        {
            if (player.PlayerInCheck)
            {
                ulong MockOfourPiecesPosition = player.PiecesPosition & ~currentPiecePosition;
                ulong MockOfOpponentPiecesPosition = opponent.PiecesPosition;
                List<IObserver> MockOfEnemyPiecesList = Clone.DeepCopyItem(opponent.PiecesList);
                ulong mockOfKingPosition = player.King.Position;
                if (piece is King)
                {
                    mockOfKingPosition = choosenPositionToMove;
                }
                if (attacked)
                {
                    foreach (IObserver observer in MockOfEnemyPiecesList.ToList())
                    {
                        BasePiece opponentPiece = observer as BasePiece;
                        if (opponentPiece.Position == choosenPositionToMove)
                        {

                            MockOfEnemyPiecesList.Remove(observer);
                            break;
                        }
                    }
                    MockOfOpponentPiecesPosition = MockOfOpponentPiecesPosition & ~choosenPositionToMove;
                }
                MockOfourPiecesPosition |= choosenPositionToMove;
                ulong opponentAttacks = Attack.GetAllOpponentAttackToCheckIfKingStillInCheck(BoardWithAllMember, MockOfOpponentPiecesPosition, MockOfourPiecesPosition, MockOfEnemyPiecesList);
                if ((opponentAttacks & mockOfKingPosition) > 0)
                {
                    Console.WriteLine("Choose another piece, the king is still in check, step not succeeded");
                    return false;
                }
                player.PlayerInCheck = false;
            }
            return true;
        }

    }
}

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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessProject.ServiceLayers
{
    public class MainGameService
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player NextPlayer { get; set; }
        public IAttack Attack { get; set; }
        public IUpdateBitBoards UpdateBitBoards { get; set; }
        public IBitScan Scan { get; set; }
        public ILongMovements Movements { get; set; }
        public IPopulationCount PopCount { get; set; }
        public ulong BoardWithAllMember = 0b_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111;
        public ObservableCollection<BasePiece> PieceCollection { get; set; }
        public String ExceptionMessage { get; set; }

        public MainGameService(ObservableCollection<BasePiece> pieceCollection, ColorSide color, IUpdateBitBoards updateBitBoards, IBitScan scan,
            ILongMovements movements, IPopulationCount popcount, IAttack attack, String exceptionMessage)
        {
            PieceCollection = pieceCollection;
            //ColorSide color = ColorSide.Black;
            //UpdateBitBoards = new UpdateBitBoards();
            //Scan = new BitScan();
            //Movements = new LongMovements();
            //PopCount = new PopulationCount();
            //Attack = new Attack(Scan, PopCount, UpdateBitBoards);
            UpdateBitBoards = updateBitBoards;
            Scan = scan;
            Movements = movements;
            PopCount = popcount;
            Attack = attack;
            ExceptionMessage = exceptionMessage;
            InitAllPieces(color, Scan, Movements, Attack);
            SelectPlayerWhoStarts(Player1, Player2);
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
                else if (i == 1 || i == 6)
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

        public void SelectPlayerWhoStarts(Player player1, Player player2)
        {
            if (player1.Color == ColorSide.White)
            {
                NextPlayer = player1;
            }
            else
            {
                NextPlayer = player2;
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

        public bool ColorCheck(Player actualPlayer, Player opponent)
        {
            if (actualPlayer.Color == opponent.Color)
            {
                return false;
            }
            return true;
        }

        public bool ProcessOfMakingSurePlayerCanChooseSpecificPiece(Player player, BasePiece piece)
        {
            Player opponent = OpponentCreater(player);
            if (!(ColorCheck(player, opponent)))
            {
                ExceptionMessage = $"Choosen piece is not {player.Color} piece"; ExceptionMessageRemover();
                NextPlayer = player;
                return false;
            }
            //IsPlayerInCheckAndCheckmateChecker(player,opponent);
            player.RecentOpportunities = piece.Search(piece.Position, BoardWithAllMember, opponent.PiecesPosition, player.PiecesPosition);
            if (player.RecentOpportunities <= 0)
            {
                ExceptionMessage = "you cannot move with this piece, choose another one"; ExceptionMessageRemover();
                player.RecentOpportunities = 0;
                NextPlayer = player;
                return false;
            }
            player.PositionsOfOpportunities = Utils.RowAndColumnCalculator.GetPositionsOfRowsAndColumns(piece.Creator.RecentOpportunities);
            return true;
        }

        public bool ProcessOfMakingSurePlayerCanDropSpecificPiece(Player player, BasePiece piece, ulong currentPiecePosition, ulong opportunities, ulong choosenPositionToMove)
        {
            Player opponent = OpponentCreater(player);
            if ((choosenPositionToMove & opportunities) <= 0)
            {
                ExceptionMessage = "You cannot move there because there is no opportunity there"; ExceptionMessageRemover();
                player.RecentOpportunities = 0;
                player.PositionsOfOpportunities.Clear();
                NextPlayer = player;
                return false;
            }

            bool attacked = Attack.HasAttacked(choosenPositionToMove, opponent.PiecesPosition);

            if (!CheckProcess(player, currentPiecePosition, opponent, piece, attacked, choosenPositionToMove))
            {
                ExceptionMessage = "Choose another piece, the king is still in check, step not succeeded"; ExceptionMessageRemover();
                NextPlayer = player;
                player.RecentOpportunities = 0;
                player.PositionsOfOpportunities.Clear();
                return false;
            }
            if (player.CheckIfCurrentAtLastLineAndIsPawn(choosenPositionToMove, piece) && player.KnockedPieces.Count != 0)
            {
                if (player == Player1)
                {
                    IsWaitedForPawnToBeSwappedToAnotherPieceForPlayer1 = true;
                    KnockedPiecesBrushOfPlayer1 = Brushes.Green;
                    ExceptionMessage = $"Choose piece {player.Color} player"; ExceptionMessageRemover();
                }
                else
                {
                    IsWaitedForPawnToBeSwappedToAnotherPieceForPlayer2 = true;
                    KnockedPiecesBrushOfPlayer2 = Brushes.Green;
                    ExceptionMessage = $"Choose piece {player.Color} player"; ExceptionMessageRemover();
                }
            }

            RemoveFromPieceCollectionBecauseAttacked(attacked, opponent, choosenPositionToMove);
            UpdateBitBoards.UpdateAllBitBoard(attacked, player, opponent, choosenPositionToMove, opportunities, currentPiecePosition, ref BoardWithAllMember);
            player.PositionsOfOpportunities.Clear();
            NextPlayer = opponent;
            IsPlayerInCheckAndCheckmateChecker(opponent, player);  // it is reversed in the method(normally the first argument is the acutal player), because we need to know the information to be displayed before the other player moves

            if (player.IsThreeFold == true || opponent.IsThreeFold == true)
            {
                ExceptionMessage = "Its a draw because of TreeFold"; ExceptionMessageRemover();
            }
            if (player.IsFiftyMoveWIthoutCaptureOrPawnMove == true || opponent.IsFiftyMoveWIthoutCaptureOrPawnMove == true)
            {
                ExceptionMessage = "Its draw because of 50 move rule"; ExceptionMessageRemover();
            }


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

                    return false;
                }
                player.PlayerInCheck = false;
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
                    ExceptionMessage = $"Check for {actualPlayer.Color} Player"; ExceptionMessageRemover();
                    actualPlayer.PlayerInCheck = true;
                    if (Attack.GetCounterAttackToChekIfSomePieceCouldEvadeAttack(opponentAttacks, actualPlayer.King.Position, BoardWithAllMember, opponent.PiecesPosition, actualPlayer.PiecesPosition, actualPlayer.PiecesList, opponent.PiecesList))
                    {
                        ExceptionMessage = $"CheckMate for {actualPlayer.Color} Player"; ExceptionMessageRemover();

                    }
                }
            }
        }
        public async void ExceptionMessageRemover()
        {
            await Task.Delay(2000);
            ExceptionMessage = "";
        }

    }
}

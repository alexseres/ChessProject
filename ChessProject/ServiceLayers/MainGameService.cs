using ChessProject.ActionLogics;
using ChessProject.ActionLogics.Attacks;
using ChessProject.ActionLogics.BitBoardsUpdater;
using ChessProject.Actions.Movements;
using ChessProject.Models;
using ChessProject.Models.Enums;
using ChessProject.Models.ObserverRelated;
using ChessProject.Models.Pieces;
using ChessProject.Utils.BitScanLogic;
using ChessProject.Utils.CloneCollections;
using ChessProject.Utils.PopulationCountLogic;
using ChessProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChessProject.ServiceLayers
{
    public class MainGameService :BaseViewModel, IMainGameService
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

        public ObservableCollection<BasePiece> _pieceCollection;
        public ObservableCollection<BasePiece> PieceCollection{ get {return _pieceCollection;} set{SetProperty(ref _pieceCollection, value);}}

        public MainGameService(Player player1, Player player2, ObservableCollection<BasePiece> pieceCollection, IUpdateBitBoards updateBitBoards, IBitScan scan,
            ILongMovements movements, IPopulationCount popcount, IAttack attack)
        {
            Player1 = player1;
            Player2 = player2;
            PieceCollection = pieceCollection;
            UpdateBitBoards = updateBitBoards;
            Scan = scan;
            Movements = movements;
            PopCount = popcount;
            Attack = attack;
            InitAllPieces(Player1.Color, Player2.Color,Scan, Movements, Attack);
            SelectPlayerWhoStarts(Player1, Player2);
        }

        public void PawnSwapperLogicForPlayer1(BasePiece piece)
        {
            PieceCollection.Remove(Player1.PawnToBeSwapped);
            Player1.SwapPawnToAnotherPiece(piece);
            PieceCollection.Add(piece);
            piece.Creator.IsWaitedForPawnToBeSwappedToAnotherPiece = false;
        }

        public void PawnSwapperLogicForPlayer2(BasePiece piece)
        {
            PieceCollection.Remove(Player2.PawnToBeSwapped);
            Player2.SwapPawnToAnotherPiece(piece);
            PieceCollection.Add(piece);
            piece.Creator.IsWaitedForPawnToBeSwappedToAnotherPiece = false;
        }

        public void InitAllPieces(ColorSide player1Color,ColorSide player2Color, IBitScan bitScan, ILongMovements movements, IAttack attack)
        {
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
                    imagePath = $"../ChessPiecePictures/{player1Color}Rook.png";
                    Rook rookUp = new Rook(Player1, player1Color, mask, bitScan, movements, attack, imagePath);
                    Player1.PiecesList.Add(rookUp);
                    Player1.PiecesPosition ^= mask;
                    PieceCollection.Add(rookUp);
                }
                else if (i == 1 || i == 6)
                {
                    imagePath = $"../ChessPiecePictures/{player1Color}Knight.png";
                    Knight knightUp = new Knight(Player1, player1Color, mask, imagePath);
                    Player1.PiecesList.Add(knightUp);
                    Player1.PiecesPosition ^= mask;
                    PieceCollection.Add(knightUp);

                }
                else if (i == 2 || i == 5)
                {
                    imagePath = $"../ChessPiecePictures/{player1Color}Bishop.png";
                    Bishop bishopUp = new Bishop(Player1, player1Color, mask, bitScan, movements, attack, imagePath);
                    Player1.PiecesList.Add(bishopUp);
                    Player1.PiecesPosition ^= mask;
                    PieceCollection.Add(bishopUp);
                }
                else if (i == 3)
                {
                    imagePath = $"../ChessPiecePictures/{player1Color}King.png";
                    King kingUp = new King(Player1, player1Color, mask, imagePath);
                    Player1.PiecesList.Add(kingUp);
                    Player1.King = kingUp;
                    Player1.PiecesPosition ^= mask;
                    PieceCollection.Add(kingUp);
                }
                else if (i == 4)
                {
                    imagePath = $"../ChessPiecePictures/{player1Color}Queen.png";
                    Queen queenUp = new Queen(Player1, player1Color, mask, bitScan, movements, attack, imagePath);
                    Player1.PiecesList.Add(queenUp);
                    Player1.PiecesPosition ^= mask;
                    PieceCollection.Add(queenUp);
                }
                else if (i >= 8 && i <= 15)
                {
                    imagePath = $"../ChessPiecePictures/{player1Color}Pawn.png";
                    Pawn pawnUp = new Pawn(Player1, player1Color, mask, imagePath, lastLineForSwapForUp, doubleMoveSignForUp, fifthLineOfEnPassantForUp);
                    Player1.PiecesList.Add(pawnUp);
                    Player1.PiecesPosition ^= mask;
                    Player2.OpponentPawnsList.Add(pawnUp);
                    PieceCollection.Add(pawnUp);
                }
                else if (i > 47 && i < 56)
                {
                    imagePath = $"../ChessPiecePictures/{player2Color}Pawn.png";
                    Pawn pawnDown = new Pawn(Player2, player2Color, mask, imagePath, lastLineForSwapToDown, doubleMoveSignForDown, fifthLineOfEnPassantForDown);
                    Player2.PiecesList.Add(pawnDown);
                    Player2.PiecesPosition ^= mask;
                    Player1.OpponentPawnsList.Add(pawnDown);
                    PieceCollection.Add(pawnDown);
                }
                else if (i == 56 || i == 63)
                {
                    imagePath = $"../ChessPiecePictures/{player2Color}Rook.png";
                    Rook rookDown = new Rook(Player2, player2Color, mask, bitScan, movements, attack, imagePath);
                    Player2.PiecesList.Add(rookDown);
                    Player2.PiecesPosition ^= mask;
                    PieceCollection.Add(rookDown);
                }
                else if (i == 57 || i == 61)
                {
                    imagePath = $"../ChessPiecePictures/{player2Color}Bishop.png";
                    Bishop bishopDown = new Bishop(Player2, player2Color, mask, bitScan, movements, attack, imagePath);
                    Player2.PiecesList.Add(bishopDown);
                    Player2.PiecesPosition ^= mask;
                    PieceCollection.Add(bishopDown);
                }
                else if (i == 58 || i == 62)
                {
                    imagePath = $"../ChessPiecePictures/{player2Color}Knight.png";
                    Knight knightDown = new Knight(Player2, player2Color, mask, imagePath);
                    Player2.PiecesList.Add(knightDown);
                    Player2.PiecesPosition ^= mask;
                    PieceCollection.Add(knightDown);
                }
                else if (i == 59)
                {
                    imagePath = $"../ChessPiecePictures/{player2Color}Queen.png";
                    Queen queenDown = new Queen(Player2, player2Color, mask, bitScan, movements, attack, imagePath);
                    Player2.PiecesList.Add(queenDown);
                    Player2.PiecesPosition ^= mask;
                    PieceCollection.Add(queenDown);
                }
                else if (i == 60)
                {
                    imagePath = $"../ChessPiecePictures/{player2Color}King.png";
                    King kingDown = new King(Player2, player2Color, mask, imagePath);
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

        public (bool,string) ProcessOfMakingSurePlayerCanChooseSpecificPiece(Player player, BasePiece piece)
        {
            string message = "";
            Player opponent = OpponentCreater(player);

            //IsPlayerInCheckAndCheckmateChecker(player,opponent);
            player.RecentOpportunities = piece.Search(piece.Position, BoardWithAllMember, opponent.PiecesPosition, player.PiecesPosition);
            if (player.RecentOpportunities <= 0)
            {
                message = "you cannot move with this piece, choose another one"; 
                player.RecentOpportunities = 0;
                NextPlayer = player;
                return (false,message);
            }
            player.PositionsOfOpportunities = Utils.RowAndColumnCalculator.GetPositionsOfRowsAndColumns(piece.Creator.RecentOpportunities);
            return (true,message);
        }

        public (bool,string) ProcessOfMakingSurePlayerCanDropSpecificPiece(Player player, BasePiece piece, ulong currentPiecePosition, ulong opportunities, ulong choosenPositionToMove)
        {
            string message = "";
            Player opponent = OpponentCreater(player);

            if ((choosenPositionToMove & opportunities) <= 0)
            {
                message = "You cannot move there because there is no opportunity there";
                player.RecentOpportunities = 0;
                player.PositionsOfOpportunities.Clear();
                NextPlayer = player;
                return (false,message);
            }

            bool attacked = Attack.HasAttacked(choosenPositionToMove, opponent.PiecesPosition);

            if (!CheckProcess(player, currentPiecePosition, opponent, piece, attacked, choosenPositionToMove))
            {
                message = "Choose another piece, the king is still in check, step not succeeded";
                NextPlayer = player;
                player.RecentOpportunities = 0;
                player.PositionsOfOpportunities.Clear();
                return (false,message);
            }

            if (player.CheckIfCurrentAtLastLineAndIsPawn(choosenPositionToMove, piece) && player.KnockedPieces.Count != 0)
            {
                player.IsWaitedForPawnToBeSwappedToAnotherPiece= true;
                message = $"Choose piece {player.Color} player"; 
            }

            RemoveFromPieceCollectionBecauseAttacked(attacked, opponent, choosenPositionToMove);
            UpdateBitBoards.UpdateAllBitBoard(attacked, player, opponent, choosenPositionToMove, opportunities, currentPiecePosition, ref BoardWithAllMember);
            player.PositionsOfOpportunities.Clear();
            NextPlayer = opponent;
            message = IsPlayerInCheckAndCheckmateChecker(opponent, player);  // it is reversed in the method(normally the first argument is the acutal player), because we need to know the information to be displayed before the other player moves
            return (true,message);
        }

        public (bool,string) CheckIfIsThreeFoldOrFiftyMove(Player player)
        {
            string message = "";
            Player opponent = OpponentCreater(player);
            if (player.IsThreeFold == true || opponent.IsThreeFold == true)
            {
                message = "Its a draw because of TreeFold"; 
                return (true,message);
            }
            if (player.IsFiftyMoveWIthoutCaptureOrPawnMove == true || opponent.IsFiftyMoveWIthoutCaptureOrPawnMove == true)
            {
                message = "Its draw because of 50 move rule";
                return (true, message);
            }
            return (false, message);

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
            ulong mockAllBoardMember = MockOfOpponentPiecesPosition | MockOfourPiecesPosition;
            ulong opponentAttacks = Attack.GetAllOpponentAttackToCheckIfKingStillInCheck(mockAllBoardMember, MockOfOpponentPiecesPosition, MockOfourPiecesPosition, MockOfEnemyPiecesList);
            if ((opponentAttacks & mockOfKingPosition) > 0)
            {

                return false;
            }
            if(player.PlayerInCheck == true) player.PlayerInCheck = false;
            return true;
        }

        public void PrintBoard(string board)
        {
            Debug.WriteLine("Bishop");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < board.Length; i++)
            {
                if (i % 8 == 0 && i != 0)
                {
                    string row = new string(sb.ToString());
                    Debug.WriteLine(row);
                    sb.Clear();
                }
                sb.Append(board[i]);
            }
            var finalRow = new string(sb.ToString());
            Debug.WriteLine(finalRow);
            Debug.WriteLine(" ");
        }

        public string IsPlayerInCheckAndCheckmateChecker(Player actualPlayer, Player opponent)
        {
            string message = "";
            if (!actualPlayer.PlayerInCheck)
            {
                ulong opponentAttacks = Attack.GetAllOpponentAttackToCheckIfKingInCheck(actualPlayer.King.Position, BoardWithAllMember, opponent.PiecesPosition, actualPlayer.PiecesPosition, opponent.PiecesList);
                if (opponentAttacks > 0)   // king in check
                {
                    message = $"Check for {actualPlayer.Color} Player"; 
                    actualPlayer.PlayerInCheck = true;
                    if (Attack.GetCounterAttackToChekIfSomePieceCouldEvadeAttack(opponentAttacks, actualPlayer.King.Position, BoardWithAllMember, opponent.PiecesPosition, actualPlayer.PiecesPosition, actualPlayer.PiecesList, opponent.PiecesList))
                    {
                        message = $"CheckMate for {actualPlayer.Color} Player"; 
                    }
                }
            }
            return message;
        }
    }
}

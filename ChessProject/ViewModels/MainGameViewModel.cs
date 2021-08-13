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
using System.Windows.Controls;

namespace ChessProject.ViewModels
{
    public class MainGameViewModel
    {
        public Rook TestRook { get; set; }

        public ObservableCollection<BasePiece> PieceCollection = new ObservableCollection<BasePiece>();
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public const string From = "Choose a piece";
        public const string To = "Move with the piece to";
        public IAttack Attack { get; set; }
        public IUpdateBitBoards UpdateBitBoards { get; set; }
        public IBitScan Scan { get; set; }
        public ILongMovements Movements { get; set; }
        public IPopulationCount PopCount { get; set; }
        public ulong BoardWithAllMember = 0b_1111_1111_1111_1111_0000_0000_0000_0000_0100_0000_0000_0000_1111_1111_1111_1111;

        public MainGameViewModel()
        {
            TestRook = new Rook(Player1, ColorSide.White, 0, new BitScan(), new LongMovements(), new Attack(new BitScan(), new PopulationCount(), new UpdateBitBoards()), "../ChessPiecePictures/WhiteRook.png");
            PieceCollection.Add(TestRook);
            //ColorSide color = SelectColorSide();
            //UpdateBitBoards = new UpdateBitBoards();
            //Scan = new BitScan();
            //Movements = new LongMovements();
            //PopCount = new PopulationCount();
            //Attack = new Attack(Scan, PopCount, UpdateBitBoards);
            //InitAllPieces(color, Scan, Movements, Attack);
            //string board = CreateStringOfBoard();
            //PrintBoard(board);
            //PlayGame();
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
                    imagePath = $"../ChessPiecePictures/{choosenColorToBeUp}Rook";
                    Rook rookUp = new Rook(Player1, choosenColorToBeUp, mask, bitScan, movements, attack, imagePath);
                    Player1.PiecesList.Add(rookUp);
                    Player1.PiecesPosition ^= mask;
                }
                else if (i == 1 || i == 6 || i == 33)
                {
                    Knight knightUp = new Knight(Player1, choosenColorToBeUp, mask, "N");
                    Player1.PiecesList.Add(knightUp);
                    Player1.PiecesPosition ^= mask;
                }
                else if (i == 2 || i == 5)
                {
                    Bishop bishopUp = new Bishop(Player1, choosenColorToBeUp, mask, bitScan, movements, attack, "B");
                    Player1.PiecesList.Add(bishopUp);
                    Player1.PiecesPosition ^= mask;
                }
                else if (i == 3)
                {
                    King kingUp = new King(Player1, choosenColorToBeUp, mask, "K");
                    Player1.PiecesList.Add(kingUp);
                    Player1.King = kingUp;
                    Player1.PiecesPosition ^= mask;
                }
                else if (i == 4)
                {
                    Queen queenUp = new Queen(Player1, choosenColorToBeUp, mask, bitScan, movements, attack, "Q");
                    Player1.PiecesList.Add(queenUp);
                    Player1.PiecesPosition ^= mask;
                }
                else if (i >= 8 && i <= 15)
                {
                    Pawn pawnUp = new Pawn(Player1, choosenColorToBeUp, mask, "P", lastLineForSwapForUp, doubleMoveSignForUp, fifthLineOfEnPassantForUp);
                    Player1.PiecesList.Add(pawnUp);
                    Player1.PiecesPosition ^= mask;
                    Player2.OpponentPawnsList.Add(pawnUp);
                }
                else if (i > 47 && i < 56)
                {
                    Pawn pawnDown = new Pawn(Player2, otherColor, mask, "H", lastLineForSwapToDown, doubleMoveSignForDown, fifthLineOfEnPassantForDown);
                    Player2.PiecesList.Add(pawnDown);
                    Player2.PiecesPosition ^= mask;
                    Player1.OpponentPawnsList.Add(pawnDown);
                }
                else if (i == 56 || i == 63)
                {
                    Rook rookDown = new Rook(Player2, otherColor, mask, bitScan, movements, attack, "A");
                    Player2.PiecesList.Add(rookDown);
                    Player2.PiecesPosition ^= mask;
                }
                else if (i == 57 || i == 61)
                {
                    Bishop bishopDown = new Bishop(Player2, otherColor, mask, bitScan, movements, attack, "S");
                    Player2.PiecesList.Add(bishopDown);
                    Player2.PiecesPosition ^= mask;
                }
                else if (i == 58 || i == 62)
                {
                    Knight knightDown = new Knight(Player2, otherColor, mask, "D");
                    Player2.PiecesList.Add(knightDown);
                    Player2.PiecesPosition ^= mask;
                }
                else if (i == 59)
                {
                    Queen queenDown = new Queen(Player2, otherColor, mask, bitScan, movements, attack, "F");
                    Player2.PiecesList.Add(queenDown);
                    Player2.PiecesPosition ^= mask;
                }
                else if (i == 60)
                {
                    King kingDown = new King(Player2, otherColor, mask, "G");
                    Player2.PiecesList.Add(kingDown);
                    Player2.PiecesPosition ^= mask;
                    Player2.King = kingDown;

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

        public void PlayGame()
        {
            bool isPlayer1AtTurn = CheckIfPlayer1IsWhite();
            while (true)
            {
                if (isPlayer1AtTurn)
                {
                    isPlayer1AtTurn = PlayerTurn(Player1, true);
                    //string board = CreateStringOfBoard();
                    //PrintBoard(board);
                }
                else
                {
                    isPlayer1AtTurn = PlayerTurn(Player2, false);
                    //string board = CreateStringOfBoard();
                    //PrintBoard(board);
                }

                if (Player1.IsThreeFold == true || Player2.IsThreeFold == true)
                {
                    Console.WriteLine("Its a draw because of TreeFold");
                    break;
                }
                if (Player1.IsFiftyMoveWIthoutCaptureOrPawnMove == true || Player2.IsFiftyMoveWIthoutCaptureOrPawnMove == true)
                {
                    Console.WriteLine("Its draw because of 50 move rule");
                    break;
                }
            }
        }

        public bool PlayerTurn(Player actualPlayer, bool isPlayer1)
        {
            Player opponent = OpponentCreater(actualPlayer);
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

            ulong choosenPos = UserInput(From);
            BasePiece choosenPiece = actualPlayer.GrabAndExtractPiece(choosenPos);
            if (choosenPiece is null || choosenPiece.Color != actualPlayer.Color)
            {
                Console.WriteLine($"Choose piece is not {actualPlayer.Color} piece");
                if (isPlayer1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                bool response = Process(actualPlayer, choosenPiece, choosenPos);

                if (response == true)
                {
                    if (isPlayer1)
                    {

                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
                else
                {
                    if (isPlayer1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
                    Console.WriteLine("Choose another piece, the king is still in check, step not succeeded");
                    return false;
                }
                player.PlayerInCheck = false;
            }
            return true;
        }


        public bool Process(Player player, BasePiece piece, ulong currentPiecePosition)
        {
            Player opponent = OpponentCreater(player);
            ulong opportunities = piece.Search(currentPiecePosition, BoardWithAllMember, opponent.PiecesPosition, player.PiecesPosition);
            if (opportunities <= 0)
            {
                Console.WriteLine("you cannot move with this piece, choose another one");
                return false;
            }

            ulong choosenPositionToMove = UserInput(To);
            if ((choosenPositionToMove & opportunities) <= 0)
            {
                Console.WriteLine("You cannot move there because there is no opportunity there");
                return false;
            }

            bool attacked = Attack.HasAttacked(choosenPositionToMove, opponent.PiecesPosition);

            if (!CheckProcess(player, currentPiecePosition, opponent, piece, attacked, choosenPositionToMove))
            {
                return false;
            }

            UpdateBitBoards.UpdateAllBitBoard(attacked, player, opponent, choosenPositionToMove, opportunities, currentPiecePosition, ref BoardWithAllMember);
            return true;
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

        public ulong UserInput(string inp)
        {
            Console.WriteLine(inp);
            string result = Console.ReadLine();
            int convertToInt = Int16.Parse(result) - 1;
            ulong pos = (ulong)1 << convertToInt;
            return pos;
        }


        #region Print and Create Board
        //public void PrintBoard(string board)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < board.Length; i++)
        //    {
        //        if (i % 8 == 0 && i != 0)
        //        {
        //            string row = new string(sb.ToString());
        //            Console.WriteLine(row);
        //            sb.Clear();
        //        }
        //        sb.Append(board[i]);
        //    }
        //    var finalRow = new string(sb.ToString());
        //    Console.WriteLine(finalRow);
        //}

        //public string CreateStringOfBoard()
        //{
        //    ulong mask = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
        //    StringBuilder sb = new StringBuilder();

        //    for (int i = 0; i < 64; i++)
        //    {
        //        bool IsSquareOccupied = false;
        //        for (int j = 0; j < Player1.PiecesList.Count; j++)
        //        {
        //            if (((Player1.PiecesList[j] as BasePiece).Position & (mask & BoardWithAllMember)) > 0)
        //            {
        //                sb.Append((Player1.PiecesList[j] as BasePiece).BoardName);
        //                IsSquareOccupied = true;
        //                break;
        //            }

        //        }
        //        for (int k = 0; k < Player2.PiecesList.Count; k++)
        //        {
        //            if (((Player2.PiecesList[k] as BasePiece).Position & (mask & BoardWithAllMember)) > 0)
        //            {
        //                sb.Append((Player2.PiecesList[k] as BasePiece).BoardName);
        //                IsSquareOccupied = true;
        //                break;
        //            }
        //        }

        //        if (!IsSquareOccupied)
        //        {
        //            sb.Append(".");
        //        }

        //        mask = mask >> 1;
        //    }
        //    Console.WriteLine(sb.ToString());
        //    return sb.ToString();
        //}
        #endregion
    }
}

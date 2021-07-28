using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class ProofOfConcept
    {
        public readonly Player Player1;
        public readonly Player Player2;
        public const string From = "Enter position to choose piece";
        public const string To = "Enter position to choose piece";
        public IAttack Attack { get; set; }


        public string[] BlackPrintedBoardNames = new string[6] { "R", "K", "B", "Q", "N", "P" };
        public ulong[] BlackInitPositions = new ulong[7] { 0b_1111_1111_1011_1111_0000_0000_0000_0000_0000_0000_0000_1000_0000_0000_0000_0000,
                                                           0b_1000_0001_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0100_0010_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0010_0100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0001_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0000_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000,
                                                            0b_0000_0000_1011_1111_0000_0000_0000_0000_0000_0000_0000_1000_0000_0000_0000_0000};

        public string[] WhitePrintedBoardNames = new string[6] { "A", "S", "D", "F", "G", "H" };
        public ulong[] WhiteInitPositions = new ulong[7] { 0b_0000_0000_0000_0000_0000_0000_0000_0000_1100_0000_0000_0010_1111_1101_1111_1111,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0010_0000_0000_1000_0001,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0100_0000_0000_0000_0000_0000_0100_0010,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0010_0100,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_1000_0000_0000_0000_0000_0000_0000_1000,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_0000,
                                                             0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1101_0000_0000 };

        public ulong BoardWithAllMember = 0b_1111_1111_1011_1111_0000_0000_0000_1000_1100_0000_0100_1010_1111_1101_1111_1111;
  
        public ProofOfConcept()
        {
            Attack = new Attack();
            Player1 = new Player(ColorSide.Black, BlackInitPositions, BlackPrintedBoardNames, new BitScan(), new LongMovements());
            Player2 = new Player(ColorSide.White, WhiteInitPositions, WhitePrintedBoardNames, new BitScan(), new LongMovements());
            string board = CreateStringOfBoard();
            PrintBoard(board);
            PlayGame();
        }

        public void PlayGame()
        {
            bool isWhiteAtTurn = false;
            while(true)
            {
                ulong choosenPos = UserInput(From);
                if (isWhiteAtTurn)
                {
                    ulong opponentAttacks = Attack.GetAllOpponentAttack(BoardWithAllMember, Player1.Pieces, Player2.Pieces, Player1.PiecesList);

                    if (Player2.KingIsInCheck(Player2.King.Positions,opponentAttacks))
                    {

                    }
                    ulong allEnemyAttack = Player2.Attack.GetAllOpponentAttack(BoardWithAllMember, Player1.Pieces, Player2.Pieces, Player1.PiecesList);
                    PrintBoard(Convert.ToString((long)allEnemyAttack, toBase: 2).PadLeft(64, '0'));
                    BasePiece choosenWhitePiece = Player2.GrabAndExtractPiece(choosenPos);
                    if (choosenWhitePiece.Color != ColorSide.White)
                    {
                        Console.WriteLine("Choose white piece not black or null");
                        isWhiteAtTurn = true;
                    }
                    else
                    {
                        bool response = Process(Player2, choosenWhitePiece, choosenPos);
                        if (response == true)
                        {
                            isWhiteAtTurn = false;
                        }
                        else
                        {
                            isWhiteAtTurn = true;
                        }
                    }
                }
                else
                {
                    ulong allEnemyAttack = Player1.Attack.GetAllOpponentAttack(BoardWithAllMember, Player2.Pieces, Player1.Pieces, Player2.PiecesList);
                    PrintBoard(Convert.ToString((long)allEnemyAttack, toBase: 2).PadLeft(64, '0'));
                    BasePiece choosenBlackPiece = Player1.GrabAndExtractPiece(choosenPos);
                    if(choosenBlackPiece.Color != ColorSide.Black)
                    {
                        Console.WriteLine("Choose black piece not white or null");
                        isWhiteAtTurn = false;
                    }
                    else
                    {
                        bool response = Process(Player1, choosenBlackPiece, choosenPos);
                        if (response == true)
                        {
                            isWhiteAtTurn = true;
                        }
                        else
                        {
                            isWhiteAtTurn = false;
                        }
                    }
                }
            }
        }
        
        public bool Process(Player player ,BasePiece piece,  ulong currentPiecePosition)
        {
            Player opponent = OpponentCreater(player);
            ulong opportunities = piece.Search(currentPiecePosition, BoardWithAllMember, opponent.Pieces, player.Pieces);
            PrintBoard(Convert.ToString((long)opportunities, toBase: 2).PadLeft(64, '0'));
            if (opportunities <= 0)
            {
                Console.WriteLine("you cannot move with this piece, choose another one");
                return false;
            }

            ulong choosenPositionToMove = UserInput(To);
            if((choosenPositionToMove & opportunities) <= 0)
            {
                Console.WriteLine("You cannot move there because there is no opportunity there");
                return false;
            }
            bool attacked = HasAttacked(choosenPositionToMove, opponent.Pieces);
            UpdateAllBitBoard(attacked, player, opponent, choosenPositionToMove, opportunities, currentPiecePosition);
            
            Console.WriteLine("updated board");
            string updatedBoard = CreateStringOfBoard();
            PrintBoard(updatedBoard);
            return true;
        }

        public void UpdateAllBitBoard(bool attacked, Player currentPlayer, Player opponent, ulong choosenPositionToMove, ulong opportunities, ulong currentPosition)
        {
            if (attacked)
            {
                opponent.NotifyBeingAttacked(choosenPositionToMove);
            }
            currentPlayer.NotifyMove(currentPosition, opportunities, choosenPositionToMove);
            BoardWithAllMember = currentPlayer.Pieces ^ opponent.Pieces;
        }


        public bool HasAttacked(ulong pos, ulong opponentPositions)
        {
            return (pos & opponentPositions) > 0 ? true : false;
        }


        public Player OpponentCreater(Player player)
        {
            if(player == Player1)
            {
                return Player2;
            }
            else
            {
                return Player1;
            }
        }


        //public BasePiece GrabAndExtractPiece(Player player, ulong pos)
        //{
            
        //    Console.WriteLine(Convert.ToString((long)pos, toBase:2).PadLeft(64,'0'));
        //    for (int i = 0;i < player.Length; i++)
        //    {
        //        Console.WriteLine(Convert.ToString((long)Player2[i].Positions, toBase:2).PadLeft(64,'0'));
        //        if ((player.Pieces & (pos & player[i].Positions)) > 0)
        //        {
        //            Console.WriteLine(" ");
        //            return player[i];
        //        }
        //    }
        //    return null;
        //}



        public ulong UserInput(string inp)
        {
            Console.WriteLine(inp);
            string result = Console.ReadLine();
            int convertToInt = Int16.Parse(result) - 1;
            ulong pos = (ulong)1 << convertToInt;
            return pos;
        }
       

        #region Print and Create Board
        public void PrintBoard(string board)
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
            var finalRow = new string(sb.ToString());
            Console.WriteLine(finalRow);   
        }

        public string CreateStringOfBoard()
        {
            ulong mask = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            StringBuilder sb = new StringBuilder();

            for(int i = 0;i< 64;i++)
            {
                bool IsSquareOccupied = false;
                for(int j = 0; j < 6; j++)
                {
                    if(((Player1.PiecesList[j] as BasePiece).Positions & (mask & BoardWithAllMember)) > 0)
                    {
                        sb.Append((Player1.PiecesList[j] as BasePiece).BoardName);
                        IsSquareOccupied = true;
                    }
                    else if (((Player2.PiecesList[j] as BasePiece).Positions & (mask & BoardWithAllMember)) > 0)
                    {
                        sb.Append((Player2.PiecesList[j] as BasePiece).BoardName);
                        IsSquareOccupied = true;
                    }
                }
                if (!IsSquareOccupied)
                {
                    sb.Append(".");
                }

                mask = mask >> 1;
            }
            Console.WriteLine(sb.ToString());
            return sb.ToString();
        }
        #endregion
    }
}
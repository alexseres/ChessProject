using ChessProject.Models.Enums;
using ChessProject.Models.ObserverRelated;
using ChessProject.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ChessProject.Models
{
    [Serializable]
    public class Player : ISubject
    {
        public PlayerType PlayerNum { get; set; }
        public ColorSide Color { get; set; }
        public ulong PiecesPosition { get; set; }
        public ObservableCollection<IObserver> KnockedPieces { get; set; }
        public King King { get; set; }
        public List<IObserver> PiecesList { get; set; }
        public List<Pawn> OpponentPawnsList { get; set; }
        public List<IObserver> OpponentPiecesList { get; set; }
        public bool IsWaitedForPawnToBeSwappedToAnotherPiece { get; set; }
        public bool IsThreeFold { get; set; } = false;
        public bool IsFiftyMoveWIthoutCaptureOrPawnMove { get; set; } = false;
        public int FiftyMoveWithoutCaptureAndPawnMove { get; set; } = 0;
        public ulong RecentOpportunities { get; set; }
        public Dictionary<int, (int, int)> PositionsOfOpportunities { get; set; }
        public bool PlayerInCheck { get; set; }
        public Pawn PawnToBeSwapped { get; set; }
        public Func<ulong, int, ulong, ulong> PawnBitwiseOperator { get; set; }
        public Func<ulong, ulong, ulong, ulong> PawnBitwiseOperatorMovedFirstPositions { get; set; }
        public Func<ulong, int, ulong, ulong> PawnBitwiseOperatorMovedPositions { get; set; }
        public int[] PawnAttackDirection new int
        // do the pawndirection for each player because is different

        public int[] PawnAttackDirection = new int[2] { 7, 9 };

        public bool HasWon { get; set; } = false;

        public Player(PlayerType type,ColorSide color)
        {
            PlayerNum = type;
            Color = color;
            KnockedPieces = new ObservableCollection<IObserver>();
            PiecesList = new List<IObserver>();
            OpponentPawnsList = new List<Pawn>();
            RecentOpportunities = 0;
            IsWaitedForPawnToBeSwappedToAnotherPiece = false;
        }

        public void Attach(IObserver observer)
        {
            this.PiecesList.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            this.PiecesList.Remove(observer);
        }

        public BasePiece GrabAndExtractPiece(ulong pos)
        {
            foreach (IObserver observer in PiecesList)
            {
                if ((PiecesPosition & (pos & (observer as BasePiece).Position)) > 0)
                {
                    return observer as BasePiece;
                }
            }
            return null;
        }

        public void NotifyBeingAttacked(ulong pos)
        {
            BasePiece attackedPiece = GrabAndExtractPiece(pos);
            attackedPiece.UpdatePositionWhenBeingAttacked();
            Detach(attackedPiece);
            KnockedPieces.Add(attackedPiece);
            PiecesPosition = PiecesPosition & ~pos;
        }

        public void NotifyMove(ulong currentPosition, ulong opportunities, ulong decidedMovePos, bool weAttacked)
        {
            BasePiece currentPiece = GrabAndExtractPiece(currentPosition);
            if (CheckIfThereWasCastling(currentPosition, currentPiece, decidedMovePos, opportunities)) return;
            if (CheckIfThereWasEnPassant(currentPosition, currentPiece, decidedMovePos) != 0)
            {
                opportunities = (opportunities & ~decidedMovePos);
                decidedMovePos = CheckIfThereWasEnPassant(currentPosition, currentPiece, decidedMovePos);
                opportunities = (opportunities | decidedMovePos);
            }
            currentPiece.UpdatePositionWhenMove(currentPosition, opportunities, decidedMovePos);
            PiecesPosition = (PiecesPosition & ~currentPosition);
            PiecesPosition = PiecesPosition ^ decidedMovePos;
            //CheckIfCurrentAtLastLineAndIsPawn(decidedMovePos, currentPiece);
            Check50MoveRule(currentPiece, weAttacked);

        }

        public void Check50MoveRule(BasePiece piece, bool weAttacked)
        {
            if (!weAttacked && !(piece is Pawn)) FiftyMoveWithoutCaptureAndPawnMove += 1;
            else FiftyMoveWithoutCaptureAndPawnMove = 0;

            if (FiftyMoveWithoutCaptureAndPawnMove == 50) IsFiftyMoveWIthoutCaptureOrPawnMove = true;
        }

        public bool CheckIfThereWasCastling(ulong currentPosition, BasePiece piece, ulong decidedMovePos, ulong kingOpportunities)
        {
            if (piece is King)
            {
                King king = piece as King;
                foreach (IObserver observer in PiecesList)
                {
                    if (observer is Rook)
                    {
                        Rook rook = observer as Rook;
                        if (rook.Position == decidedMovePos)
                        {

                            ulong newKingPos = 0;
                            ulong newRookPos = 0;
                            if (king.Position > rook.Position)
                            {
                                if (king.Position >> 4 == rook.Position)
                                {
                                    newKingPos = king.Position >> 3;
                                    newRookPos = rook.Position << 2;
                                }
                                else if (king.Position >> 3 == rook.Position)
                                {
                                    newKingPos = king.Position >> 2;
                                    newRookPos = rook.Position << 2;
                                }
                            }
                            else
                            {
                                if (king.Position << 4 == rook.Position)
                                {
                                    newKingPos = king.Position << 3;
                                    newRookPos = rook.Position >> 2;
                                }
                                else if (king.Position << 3 == rook.Position)
                                {
                                    newKingPos = king.Position << 2;
                                    newRookPos = rook.Position >> 2;
                                }
                            }
                            kingOpportunities |= newKingPos;
                            PiecesPosition = PiecesPosition & ~king.Position;
                            PiecesPosition = PiecesPosition & ~rook.Position;
                            king.UpdatePositionWhenMove(currentPosition, kingOpportunities, newKingPos);
                            rook.UpdatePositionWhenMove(rook.Position, newRookPos, newRookPos);
                            PiecesPosition = PiecesPosition ^ king.Position;
                            PiecesPosition = PiecesPosition ^ rook.Position;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public ulong CheckIfThereWasEnPassant(ulong currentPosition, BasePiece piece, ulong decidedMovePos)
        {
            if (piece is Pawn)
            {
                Pawn pawn = piece as Pawn;
                bool wasEnpassant = false;
                if ((currentPosition >> 1 & decidedMovePos) > 0)
                {
                    wasEnpassant = true;
                }
                if ((currentPosition << 1 & decidedMovePos) > 0)
                {
                    wasEnpassant = true;
                }
                if (wasEnpassant) return pawn.Color == ColorSide.Black ? decidedMovePos >> 8 : decidedMovePos << 8;
            }
            return 0;
        }


        public void SwapPawnToAnotherPiece(BasePiece piece)
        {
            piece.Position = PawnToBeSwapped.Position;
            piece.CalculateRowAndColumnPosition(PawnToBeSwapped.Position);

            PawnToBeSwapped.UpdatePositionWhenBeingAttacked();
            Detach(PawnToBeSwapped);
            KnockedPieces.Add(PawnToBeSwapped);
            PawnToBeSwapped = null;

            Attach(piece);
            KnockedPieces.Remove(piece);

        }
        public bool CheckIfCurrentAtLastLineAndIsPawn(ulong currentPosition, BasePiece currentPiece)
        {
            if (currentPiece is Pawn)
            {
                Pawn pawn = currentPiece as Pawn;
                if ((currentPosition & pawn.LastLine) > 0)
                {
                    PawnToBeSwapped = pawn;
                    return true;
                    //if (PromptAskingWhichPieceYouWantToSwap(currentPosition))
                    //{
                    //    pawn.UpdatePositionWhenBeingAttacked();
                    //    Detach(pawn);
                    //    KnockedPieces.Add(pawn);
                    //}
                }
            }
            return false;
        }


        public bool PromptAskingWhichPieceYouWantToSwap(ulong currentPosition)
        {
            Console.WriteLine("Do you want to swap?  'yes'  or 'no'");
            string answer = Console.ReadLine();
            int counter = 0;
            if (answer == "yes")
            {
                //Console.WriteLine("Please select from the list");
                //foreach (BasePiece piece in KnockedPieces)
                //{
                //    Console.WriteLine(piece.PType);
                //    counter++;

                //}

                //if (counter == 0)
                //{
                //    Console.WriteLine("no pieces available to swap");
                //    return false;
                //}
                string pieceName = Console.ReadLine();
                foreach (BasePiece piece in KnockedPieces.ToList())
                {
                    if (pieceName == piece.ImagePath)
                    {
                        piece.Position = currentPosition;
                        piece.CalculateRowAndColumnPosition(currentPosition);
                        Attach(piece);
                        KnockedPieces.Remove(piece);
                        return true;
                    }
                }
                Console.WriteLine("Wrong name you have given");
            }
            return false;
        }

        

        //indexer of the class
        //public BasePiece this[int index]
        //{
        //    get => PiecesList[index];
        //}
        //public int Length => PiecesList.Count;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace ChessProgrammingFundamentalsPractice
{
    [Serializable]
    public class Player : ISubject
    {
        public ColorSide Color { get; set; }
        public ulong PiecesPosition { get; set; }
        public List<IObserver> KnockedPieces { get; set; }
        public King King { get; set; }
        public List<IObserver> PiecesList { get; set; }
        public List<Pawns> OpponentPawnsList { get; set; }
        public List<IObserver> OpponentPiecesList { get; set; }

        public bool PlayerInCheck { get; set; }

        public Player(ColorSide color)
        {
            Color = color;
            KnockedPieces = new List<IObserver>();
            PiecesList = new List<IObserver>();
            OpponentPawnsList = new List<Pawns>();
        }
            
        public void Attach(IObserver observer)
        {
            Console.WriteLine("attached an observer");
            this.PiecesList.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            this.PiecesList.Remove(observer);
        }

        public BasePiece GrabAndExtractPiece(ulong pos)
        {
            foreach(IObserver observer in PiecesList)
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

        public void NotifyMove(ulong currentPosition, ulong opportunities, ulong decidedMovePos)
        {
            BasePiece currentPiece = GrabAndExtractPiece(currentPosition);
            if (CheckIfThereWasCastling(currentPosition, currentPiece, decidedMovePos, opportunities)) return;
            if(CheckIfThereWasEnPassant(currentPosition, currentPiece, decidedMovePos) != 0)
            {
                opportunities = (opportunities & ~decidedMovePos);
                decidedMovePos = CheckIfThereWasEnPassant(currentPosition, currentPiece, decidedMovePos);
                opportunities = (opportunities | decidedMovePos);
            }

            currentPiece.UpdatePositionWhenMove(currentPosition, opportunities, decidedMovePos);
            PiecesPosition = (PiecesPosition & ~currentPosition);
            PiecesPosition = PiecesPosition ^ decidedMovePos;
            CheckIfCurrentAtLastLineAndIsPawn(decidedMovePos, currentPiece);
        }

        public bool CheckIfThereWasCastling(ulong currentPosition, BasePiece piece, ulong decidedMovePos, ulong kingOpportunities)
        {
            if(piece is King)
            {
                King king = piece as King;
                foreach(IObserver observer in PiecesList)
                {
                    if(observer is Rooks)
                    {
                        Rooks rook = observer as Rooks;
                        if(rook.Position == decidedMovePos)
                        {

                            ulong newKingPos = 0;
                            ulong newRookPos = 0;
                            if(king.Position > rook.Position)
                            {
                                if(king.Position >> 4 == rook.Position)
                                {
                                    newKingPos = king.Position >> 3;
                                    newRookPos = rook.Position << 2;
                                }
                                else if(king.Position >> 3 == rook.Position)
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
            if(piece is Pawns)
            {
                Pawns pawn = piece as Pawns;
                bool wasEnpassant = false;
                if((currentPosition >> 1 & decidedMovePos) > 0)
                {
                    wasEnpassant = true; 
                }
                if ((currentPosition << 1 & decidedMovePos) > 0)
                {
                    wasEnpassant = true;
                }
                if(wasEnpassant) return pawn.Color == ColorSide.Black ? decidedMovePos >> 8 : decidedMovePos << 8; 
            }
            return 0;
        }

        public void CheckIfCurrentAtLastLineAndIsPawn(ulong currentPosition, BasePiece currentPiece)
        {
            if (currentPiece is Pawns)
            {
                Pawns pawn = currentPiece as Pawns;
                if ((currentPosition & pawn.LastLine) > 0)
                {
                    if (PromptAskingWhichPieceYouWantToSwap(currentPosition))
                    {
                        pawn.UpdatePositionWhenBeingAttacked();
                        Detach(pawn);
                        KnockedPieces.Add(pawn);
                    }
                }
            }
        }


        public bool PromptAskingWhichPieceYouWantToSwap(ulong currentPosition)
        {
            Console.WriteLine("Do you want to swap?  'yes'  or 'no'");
            string answer = Console.ReadLine();
            int counter = 0;
            if(answer == "yes")
            {
                Console.WriteLine("Please select from the list");
                foreach(BasePiece piece in KnockedPieces)
                {
                    Console.WriteLine(piece.Name);
                    counter++;
                    
                }
                
                if(counter == 0)
                {
                    Console.WriteLine("no pieces available to swap");
                    return false;
                }
                string pieceName = Console.ReadLine();
                foreach(BasePiece piece in KnockedPieces.ToList())
                {
                    if(pieceName == piece.BoardName)
                    {
                        piece.Position = currentPosition;
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

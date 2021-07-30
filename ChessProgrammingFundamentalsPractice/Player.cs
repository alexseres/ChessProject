﻿using System;
using System.Collections.Generic;
using System.Text;



namespace ChessProgrammingFundamentalsPractice
{
    public class Player : ISubject
    {
        public ColorSide Color { get; set; }
        public ulong Pieces { get; set; }
        public Rooks Rooks { get; set; }
        public Knights Knights { get; set; } 
        public Bishops Bishops { get; set; } 
        public Queen Queen { get; set; }  
        public King King { get; set; } 
        public Pawns Pawns { get; set; }

        public Dictionary<string, int> KnockedPieces { get; set; }

        public List<IObserver> PiecesList { get; set; }

        public Player(ColorSide color, ulong[] positions, string[] namesOfPiecesOnPrintedBoard, IBitScan bitscan, ILongMovements movements)
        {
            Color = color;
            Pieces = positions[0];
            Rooks = new Rooks(color, positions[1], bitscan, movements, new Attack());
            Knights = new Knights(color, positions[2]);
            Bishops = new Bishops(color, positions[3], bitscan, movements, new Attack());
            Queen = new Queen(color, positions[4], bitscan, movements, new Attack());
            King = new King(color, positions[5]);
            Pawns = new Pawns(color, positions[6], BorderOrganizer.OrganizeOrder(color));
            KnockedPieces = new Dictionary<string, int>() { { "Rooks", 0}, { "Bishops", 0}, { "Knights", 0}, { "Pawns",0 }, {"Queen",0 }, { "King",0} };
            PiecesList = new List<IObserver>() { Rooks, Knights, Bishops, Queen, King, Pawns };
            InitPieces(namesOfPiecesOnPrintedBoard);
        }


        private void InitPieces(string[] names)
        {
            for (int i = 0; i < PiecesList.Count; i++)
            {
                (PiecesList[i] as BasePiece).BoardName = names[i];
            }
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
                if ((Pieces & (pos & (observer as BasePiece).Positions)) > 0)
                {
                    return observer as BasePiece;
                }
            }
            return null;
        }

        public void NotifyBeingAttacked(ulong pos)
        {
            BasePiece attackedPiece = GrabAndExtractPiece(pos);
            attackedPiece.UpdatePositionWhenBeingAttacked(pos);
            Pieces = Pieces & ~pos;
            UpdateKnockedOutPiece(attackedPiece);

        }

        public void UpdateKnockedOutPiece(BasePiece piece)
        {
            if (KnockedPieces.ContainsKey(piece.GetType().Name))
            {
                KnockedPieces[piece.GetType().Name] += 1;
            }
            else
            {
                Console.WriteLine("You given wrong name");
            }
        }

        public void NotifyMove(ulong currentPosition, ulong opportunities, ulong decidedMovePos)
        {
            BasePiece currentPiece = GrabAndExtractPiece(currentPosition);
            Pieces = (Pieces & ~currentPosition);
            currentPiece.UpdatePositionWhenMove(currentPosition, opportunities, decidedMovePos);
            Pieces = Pieces ^ decidedMovePos;
            CheckIfCurrentAtLastLineAndIsPawn(currentPosition, currentPiece);
        }

        public void CheckIfCurrentAtLastLineAndIsPawn(ulong currentPosition, BasePiece currentPiece)
        {
            if (currentPiece is Pawns)
            {
                Pawns pawn = currentPiece as Pawns;
                if ((currentPosition & pawn.LastLine) > 0)
                {
                    PromptAskingWhichPieceYouWantToSwap(currentPosition);

                }
            }
        }


        public void PromptAskingWhichPieceYouWantToSwap(ulong currentPosition)
        {
            Console.WriteLine("Do you want to swap?  'yes'  or 'no'");
            string answer = Console.ReadLine();
            if(answer == "yes")
            {
                Console.WriteLine("Please select from the list");
                foreach(var item in KnockedPieces)
                {
                    Console.Write(item.Key);
                }
                Console.WriteLine();

                string pieceName = Console.ReadLine();
                if (KnockedPieces.ContainsKey(pieceName))
                {
                    KnockedPieces[pieceName] -= 1;

                    //next line is create an instance only providing its string name
                    Console.WriteLine("select pieceName");
                    string name = Console.ReadLine();
                    Type t = Type.GetType($"ChessProgrammingFundamentalsPractice.{name}");

                    foreach (IObserver observer in PiecesList)
                    {
                        BasePiece choosenPiece = observer as BasePiece;
                        if (choosenPiece.GetType().Name == t.Name)
                        {
                            choosenPiece.Positions |= currentPosition;
                        }

                    }
                }
            }
        }

        //indexer of the class
        //public BasePiece this[int index]
        //{
        //    get => PiecesList[index];
        //}
        //public int Length => PiecesList.Count;

    }
}

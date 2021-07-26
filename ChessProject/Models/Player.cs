using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.Models
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

        public List<IObserver> PiecesList;

        public Player(ColorSide color, ulong[] positions, string[] namesOfPiecesOnPrintedBoard, IBitScan bitscan, ILongMovements movements, IAttack rayAttack)
        {
            Color = color;
            Pieces = positions[0];
            Rooks = new Rooks(color, positions[1], bitscan, movements, rayAttack);
            Knights = new Knights(color, positions[2]);
            Bishops = new Bishops(color, positions[3], bitscan, movements, rayAttack);
            Queen = new Queen(color, positions[4], bitscan, movements, rayAttack);
            King = new King(color, positions[5]);
            Pawns = new Pawns(color, positions[6]);
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
            foreach (IObserver observer in PiecesList)
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
        }

        public void NotifyMove(ulong currentPosition, ulong opportunities, ulong decidedMovePos)
        {
            BasePiece currentPiece = GrabAndExtractPiece(currentPosition);
            Pieces = (Pieces & ~currentPiece.Positions);
            currentPiece.UpdatePositionWhenMove(currentPosition, opportunities, decidedMovePos);
            Pieces = Pieces ^ currentPiece.Positions;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;



namespace ChessProgrammingFundamentalsPractice
{
    public class Player
    {
        public ColorSide Color { get; set; }
        public ulong Pieces { get; set; }
        public Rooks Rooks { get; set; }
        public Knights Knights { get; set; } 
        public Bishops Bishops { get; set; } 
        public Queen Queen { get; set; }  
        public King King { get; set; } 
        public Pawns Pawns { get; set; }

        List<BasePiece> PiecesList;
        public Player(ColorSide color, ulong[] positions, string[] namesOfPiecesOnPrintedBoard)
        {
            Color = color;
            Pieces = positions[0];
            Rooks = new Rooks(color)
            {
                Positions = positions[1]
            };
            Knights = new Knights(color)
            {
                Positions = positions[2]
            };
            Bishops = new Bishops(color)
            {
                Positions = positions[3]
            };
            Queen = new Queen(color)
            {
                Positions = positions[4]
            };
            King = new King(color)
            {
                Positions = positions[5]
            };
            Pawns = new Pawns(color)
            {
                Positions = positions[6]
            };

            PiecesList = new List<BasePiece>() { Rooks, Knights, Bishops, Queen, King, Pawns };
            InitPieces(namesOfPiecesOnPrintedBoard);

        }

        

        private void InitPieces(string[] names)
        {
            for(int i = 0;i < PiecesList.Count; i++)
            {
                PiecesList[i].BoardName = names[i];
            }
        }

        public BasePiece this[int index]
        {
            get => PiecesList[index];
        }

        public int Length => PiecesList.Count;
        
    }
}

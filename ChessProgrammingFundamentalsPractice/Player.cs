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
            Rooks = new Rooks(color, positions[1], new BitScan(), new LongMovements());
            Knights = new Knights(color, positions[2]);
            Bishops = new Bishops(color, positions[3], new BitScan(), new LongMovements());
            Queen = new Queen(color, positions[4]);
            King = new King(color, positions[5]);
            Pawns = new Pawns(color, positions[6]);

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

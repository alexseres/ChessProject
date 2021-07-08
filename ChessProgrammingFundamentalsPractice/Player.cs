using System;
using System.Collections.Generic;
using System.Text;



namespace ChessProgrammingFundamentalsPractice
{
    public class Player
    {
        public ColorSide Color { get; set; }
        public ulong Pieces { get; set; }
        public Rooks Rooks { get; set; } = new Rooks();
        public Knights Knights { get; set; } = new Knights();
        public Bishops Bishops { get; set; } = new Bishops();
        public Queen Queen { get; set; } = new Queen();
        public King King { get; set; } = new King();
        public Pawns Pawns { get; set; }

        List<BasePiece> PiecesList;
        public Player(ColorSide color, ulong[] positions, string[] namesOfPiecesOnPrintedBoard)
        {
            Color = color;
            PawnInitializer(color);
            Pieces = positions[0];
            Rooks.Positions = positions[1];
            Knights.Positions = positions[2];
            Bishops.Positions = positions[3];
            Queen.Positions = positions[4];
            King.Positions = positions[5];
            Pawns.Positions = positions[6];

            PiecesList = new List<BasePiece>() { Rooks, Knights, Bishops, Queen, King, Pawns };
            InitPieces(namesOfPiecesOnPrintedBoard);

        }

        public void PawnInitializer(ColorSide color)
        {

            if(color == ColorSide.Black)
            {
                int route = -8;
                Pawns = new Pawns(route);
            }
            else
            {
                int route = 8;
                Pawns = new Pawns(route);
            }
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

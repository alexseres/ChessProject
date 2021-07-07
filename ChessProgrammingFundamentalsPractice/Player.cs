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
        public Pawns Pawns { get; set; } = new Pawns();

        public Player(ColorSide color, ulong[] positions, string[] namesOfPiecesOnPrintedBoard)
        {
            Color = color;
            Pieces = positions[0];
            Rooks.Positions = positions[1];
            Knights.Positions = positions[2];
            Bishops.Positions = positions[3];
            Queen.Positions = positions[4];
            King.Positions = positions[5];
            Pawns.Positions = positions[6];


            PiecesList = new List<BasePiece>() { Rooks, Knights, Bishops, Queen, King, Pawns };
            NamesOfPiecesOnPrintedBoard = namesOfPiecesOnPrintedBoard;
            InitPiecesPosAndNames();

        }


        #region IndexersRelated
        List<BasePiece> PiecesList;

        readonly (string, ulong)[] PiecesPositinsAndNames = new (string, ulong)[6];
        public string[] NamesOfPiecesOnPrintedBoard { get; set; }


        private void InitPiecesPosAndNames()
        {
            for(int i = 0;i < PiecesList.Count; i++)
            {
                PiecesPositinsAndNames[i] = (NamesOfPiecesOnPrintedBoard[i], PiecesList[i].Positions);
            }
        }

        public (string, ulong) this[int index]
        {
            get => PiecesPositinsAndNames[index];
        }

        public int Length => PiecesPositinsAndNames.Length;
        #endregion
    }
}

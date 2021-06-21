using ChessProject.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChessProject.Services
{
    /// <summary>
    /// this class creates all the pieces we need for one side
    /// dont need to mention why I implemented without writing wow many pieces we need from each
    /// </summary>
    public static class SideInitializer
    {
        public static Side InitializeSide(string sideName)
        {
            Side side = new Side();
            side.Name = sideName;
            side.Pieces = new ObservableCollection<BasePiece>();
            side.Pieces.Add(new Pawn());
            side.Pieces.Add(new Pawn());
            side.Pieces.Add(new Pawn());
            side.Pieces.Add(new Pawn());
            side.Pieces.Add(new Pawn());
            side.Pieces.Add(new Pawn());
            side.Pieces.Add(new Pawn());
            side.Pieces.Add(new Pawn());
            side.Pieces.Add(new Bishop());
            side.Pieces.Add(new Bishop());
            side.Pieces.Add(new Knight());
            side.Pieces.Add(new Knight());
            side.Pieces.Add(new Rook());
            side.Pieces.Add(new Rook());
            side.Pieces.Add(new Queen());
            side.Pieces.Add(new King());
            return side;
        }
    }
}

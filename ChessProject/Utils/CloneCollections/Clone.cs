using ChessProject.Models.ObserverRelated;
using ChessProject.Models.Pieces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ChessProject.Utils.CloneCollections
{
    public static class Clone
    {


        public static List<BasePiece> ClonePieces(List<IObserver> enemyPieceList)
        {
            List<BasePiece> enemyClonePiecesList = new List<BasePiece>();

            foreach(IObserver observer in enemyPieceList)
            {
                BasePiece piece = observer as BasePiece;
                if(piece is King)
                {
                    King king = piece as King;
                    King finaKing = new King(piece.Creator, piece.Creator.Color, piece.Position, piece.ImagePath);
                    finaKing.LatestMove = king.LatestMove;
                    finaKing.OpponentKing = king.OpponentKing;
                    enemyClonePiecesList.Add(finaKing);
                }
                if(piece is Pawn)
                {
                    Pawn pawn = piece as Pawn;
                    Pawn finalPawn = new Pawn(pawn.Creator, pawn.Creator.Color, pawn.Position, pawn.ImagePath, pawn.LastLine, pawn.MaskOfDoubleMove, pawn.FifthLineOfEnPassant);
                    finalPawn.LatestMove = pawn.LatestMove;
                    enemyClonePiecesList.Add(finalPawn);
                }
                if(piece is Knight)
                {
                    Knight knight = piece as Knight;
                    Knight finalKnight = new Knight(knight.Creator, knight.Creator.Color, knight.Position, knight.ImagePath);
                    finalKnight.LatestMove = knight.LatestMove;
                    enemyClonePiecesList.Add(finalKnight);
                }
                if(piece is Bishop)
                {
                    Bishop bishop = piece as Bishop;
                    Bishop finalBishop = new Bishop(bishop.Creator, bishop.Creator.Color, bishop.Position, bishop.BitScan, bishop.Movements, bishop.Attack, bishop.ImagePath);
                    finalBishop.LatestMove = bishop.LatestMove;
                    enemyClonePiecesList.Add(finalBishop);
                }
                if(piece is Rook)
                {
                    Rook rook = piece as Rook;
                    Rook finalRook = new Rook(rook.Creator, rook.Creator.Color, rook.Position, rook.BitScan, rook.Movements, rook.Attack, rook.ImagePath);
                    finalRook.LatestMove = rook.LatestMove;
                    enemyClonePiecesList.Add(finalRook);
                }
                if(piece is Queen)
                {
                    Queen queen = piece as Queen;
                    Queen finalQueen = new Queen(queen.Creator, queen.Creator.Color, queen.Position, queen.BitScan, queen.Movements, queen.Attack, queen.ImagePath);
                    finalQueen.LatestMove = queen.LatestMove;
                    enemyClonePiecesList.Add(finalQueen);
                }
            }
            return enemyClonePiecesList;
        }

        public static List<BasePiece> ConvertIObserverToBasePieceList(List<IObserver> observers)
        {
            List<BasePiece> pieces = new List<BasePiece>();
            foreach(IObserver observer in observers)
            {
                BasePiece piece = observer as BasePiece;
                pieces.Add(piece);
            }
            return pieces;
        }

        public static T DeepCopyItem<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            T result = (T)formatter.Deserialize(stream);
            stream.Close();
            return result;
        }

        public static IList<T> CloneList<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}

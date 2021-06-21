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
    /// Also it is hardcoded because the basic formation of chess always constant, never changes
    /// </summary>
    public static class SideInitializer
    {
        public static Side CreatePieces(string sideName, string sidePosition)
        {
            //do not change the order of the elements!
            Side side = new Side();
            side.Name = sideName;
            side.SidePosition = sidePosition;
            Positions[] sidePositions = DefineInitSidePosition(sidePosition);
            side.Pieces = new ObservableCollection<BasePiece>();
            side.Pieces.Add(new Rook() { InitPositions = sidePositions[0] });
            side.Pieces.Add(new Knight() { InitPositions = sidePositions[1] });
            side.Pieces.Add(new Bishop() { InitPositions = sidePositions[2] });
            side.Pieces.Add(new Queen() { InitPositions = sidePositions[3] });
            side.Pieces.Add(new King() { InitPositions = sidePositions[4] });
            side.Pieces.Add(new Bishop() { InitPositions = sidePositions[5] });
            side.Pieces.Add(new Knight() { InitPositions = sidePositions[6] });
            side.Pieces.Add(new Rook() { InitPositions = sidePositions[7] });
            side.Pieces.Add(new Pawn() { InitPositions = sidePositions[8] });
            side.Pieces.Add(new Pawn() { InitPositions = sidePositions[9] });
            side.Pieces.Add(new Pawn() { InitPositions = sidePositions[10] });
            side.Pieces.Add(new Pawn() { InitPositions = sidePositions[11] });
            side.Pieces.Add(new Pawn() { InitPositions = sidePositions[12] });
            side.Pieces.Add(new Pawn() { InitPositions = sidePositions[13] });
            side.Pieces.Add(new Pawn() { InitPositions = sidePositions[14] });
            side.Pieces.Add(new Pawn() { InitPositions = sidePositions[15] });
            return side;
        }

        public static Positions[] DefineInitSidePosition(string side)
        {
            //do not change the order
            Positions[] positions = new Positions[15];
            if(side == "upside")
            {
                positions[0] = new Positions { Column = "A", Row = "1" };
                positions[1] = new Positions { Column = "B", Row = "1" };
                positions[2] = new Positions { Column = "C", Row = "1" };
                positions[3] = new Positions { Column = "D", Row = "1" };
                positions[4] = new Positions { Column = "E", Row = "1" };
                positions[5] = new Positions { Column = "F", Row = "1" };
                positions[6] = new Positions { Column = "G", Row = "1" };
                positions[7] = new Positions { Column = "H", Row = "1" };
                positions[8] = new Positions { Column = "A", Row = "2" };
                positions[9] = new Positions { Column = "B", Row = "2" };
                positions[10] = new Positions { Column = "C", Row = "2" };
                positions[11] = new Positions { Column = "D", Row = "2" };
                positions[12] = new Positions { Column = "E", Row = "2" };
                positions[13] = new Positions { Column = "F", Row = "2" };
                positions[14] = new Positions { Column = "G", Row = "2" };
                positions[15] = new Positions { Column = "H", Row = "2" };
            }
            else if(side == "downside")
            {
                positions[0] = new Positions { Column = "A", Row = "7" };
                positions[1] = new Positions { Column = "B", Row = "7" };
                positions[2] = new Positions { Column = "C", Row = "7" };
                positions[3] = new Positions { Column = "D", Row = "7" };
                positions[4] = new Positions { Column = "E", Row = "7" };
                positions[5] = new Positions { Column = "F", Row = "7" };
                positions[6] = new Positions { Column = "G", Row = "7" };
                positions[7] = new Positions { Column = "H", Row = "7" };
                positions[8] = new Positions { Column = "A", Row = "8" };
                positions[9] = new Positions { Column = "B", Row = "8" };
                positions[10] = new Positions { Column = "C", Row = "8" };
                positions[11] = new Positions { Column = "E", Row = "8" };
                positions[12] = new Positions { Column = "D", Row = "8" };
                positions[13] = new Positions { Column = "F", Row = "8" };
                positions[14] = new Positions { Column = "G", Row = "8" };
                positions[15] = new Positions { Column = "H", Row = "8" };
            }
            else
            {
                throw new Exception("wrong pitchside name added or not specified");
            }

            return positions;
        }
    }
}

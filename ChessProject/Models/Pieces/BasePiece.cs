using ChessProject.Models.Enums;
using ChessProject.Models.ObserverRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ChessProject.Models.Pieces
{
    [Serializable]
    public abstract class BasePiece : IObserver
    {
        public Player Creator { get; set; }
        public ulong Position { get; set; }
        public PieceType PType { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public string ImagePath { get; set; }
        public ColorSide Color { get; set; }
        public List<ulong> AllMovesHasTaken { get; set; }

        //      from    to
        public (ulong, ulong) LatestMove { get; set; }

        public BasePiece(Player player, ColorSide color, ulong position, string imagePath)
        {
            Creator = player;
            ImagePath = imagePath;
            Color = color;
            Position = position;
            CalculateRowAndColumnPosition(position);
            LatestMove = (0, 0);
            AllMovesHasTaken = new List<ulong>();
        }

        public abstract ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions);

        public virtual ulong GetSpecificAttackFromSearch(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions, ulong opponentPiecePosition)
        {
            return 0;
        }


        public void CalculateRowAndColumnPosition(ulong actualPosition)
        {
            ulong mask = 0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            for (int row = 0; row <= 7; row++)
            {
                for (int col = 0; col <= 7; col++)
                {
                    if ((mask & actualPosition) > 0)
                    {
                        Row = row;
                        Column = col;
                    }
                    mask = mask >> 1;
                }
            }
        }

        public void CheckForThreeFoldRepetition()
        {
            if (AllMovesHasTaken.Count > 5)
            {
                int count = AllMovesHasTaken.Count - 1;
                ulong last = AllMovesHasTaken[count];
                if (last == AllMovesHasTaken[count - 2] && last == AllMovesHasTaken[count - 4]) Creator.IsThreeFold = true;
            }
        }

        public void UpdatePositionWhenMove(ulong currentPosition, ulong opportunities, ulong decidedMovePos)
        {

            if ((opportunities & decidedMovePos) > 0)
            {
                Position = (~currentPosition & Position) | decidedMovePos;
                AllMovesHasTaken.Add(decidedMovePos);
                //for now we need latest move to en passant and/or castling
                LatestMove = (currentPosition, decidedMovePos);
                CalculateRowAndColumnPosition(decidedMovePos);
            }
            CheckForThreeFoldRepetition();
        }

        public void UpdatePositionWhenBeingAttacked()
        {
            Position = 0;
            AllMovesHasTaken.Clear();
            Column = 0;
            Row = 0;
        }
    }
}

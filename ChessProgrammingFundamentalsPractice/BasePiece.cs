using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    [Serializable]
    public abstract class BasePiece : IObserver
    {

        public int Column { get; set; }
        public int Row { get; set; }
        public Player Creator { get; set; }
        public ulong Position { get; set; }
        public string Name { get; set; }
        public string BoardName { get; set; }
        public ColorSide Color { get; set; }
        public List<ulong> AllMovesHasTaken { get; set; }

        //      from    to
        public (ulong, ulong) LatestMove { get; set; }


        public BasePiece(Player player, ColorSide color, ulong position, string boardname)
        { 
            Creator = player;
            BoardName = boardname;
            Color = color;
            Position = position;

            LatestMove = (0, 0);
            AllMovesHasTaken = new List<ulong>();
        }

        public abstract ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions);

        public virtual ulong GetSpecificAttackFromSearch(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions, ulong opponentPiecePosition)
        {
            return 0;
        }



        public void CheckForThreeFoldRepetition()
        {
            if(AllMovesHasTaken.Count > 5)
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

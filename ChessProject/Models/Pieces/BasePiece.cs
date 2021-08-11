using ChessProject.Models.ObserverRelated;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.Models.Pieces
{
    [Serializable]
    public abstract class BasePiece : IObserver
    {
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

        public void Printboard(string board)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < board.Length; i++)
            {
                if (i % 8 == 0 && i != 0)
                {
                    string row = new string(sb.ToString());
                    Console.WriteLine(row);
                    sb.Clear();
                }
                sb.Append(board[i]);
            }
            var finalrow = new string(sb.ToString());
            Console.WriteLine(finalrow);
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
            }
            CheckForThreeFoldRepetition();
        }

        public void UpdatePositionWhenBeingAttacked()
        {
            Position = 0;
            AllMovesHasTaken.Clear();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject
{
    public abstract class BasePiece : IObserver
    {
        public ulong Positions { get; set; }
        public string Name { get; set; }
        public string BoardName { get; set; }

        public ColorSide Color { get; set; }

        public BasePiece(ColorSide color, ulong positions)
        {
            Color = color;
            Positions = positions;
        }

        public abstract ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions);

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
        public void UpdatePositionWhenMove(ulong currentPosition, ulong opportunities, ulong decidedMovePos)
        {
            if ((opportunities & decidedMovePos) > 0)
            {
                Positions = (~currentPosition & Positions) | decidedMovePos;
            }
        }

        public void UpdatePositionWhenBeingAttacked(ulong attackedPosition)
        {
            if ((attackedPosition & Positions) > 0)
            {
                Positions = (Positions & ~attackedPosition);
            }
        }

    }
}

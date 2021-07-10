using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class Pawns : BasePiece
    {
        public const int MovingDirection = 8;
        public readonly int[] AttacksDirection = new int[2] { 7, 9 };

        //this mask checks if pawns at the starting position, and if they are, they got the chance to move 2 square
        public readonly ulong MaskOfDoubleMove;

        public Pawns(ColorSide color) : base(color)
        {
            MaskOfDoubleMove = color == ColorSide.Black ? (ulong)0b_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000 : 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000; 
        }
         
        public override void Move(int pos)
        {
                 
        }

        public void Attack()
        {

        }

        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            if(this.Color == ColorSide.Black)
            {

                ulong attackPositions = (currentPosition >> AttacksDirection[0]) ^ (currentPosition >> AttacksDirection[1]);
                
                //this one checks if we want to move double square and noone is in front of us
                ulong movedFirstPositions = (((MaskOfDoubleMove & currentPosition) >> 8) & ~allPositionAtBoard) >> 8;
                ulong movedPositions = (currentPosition >> MovingDirection) | movedFirstPositions;
                ulong opportunities = (~allPositionAtBoard & movedPositions) | ((~ourPositions & attackPositions) & opponentPositionAtBoard);
                return opportunities;
            }
            else
            {
                ulong attackPositions = (currentPosition << AttacksDirection[0]) ^ (currentPosition << AttacksDirection[1]);

                //this one checks if we want to move double square and noone is in front of us
                ulong movedFirstPositions = (((MaskOfDoubleMove & currentPosition) << 8) & ~allPositionAtBoard) << 8;
                ulong movedPositions = (currentPosition << MovingDirection) | movedFirstPositions;
                ulong opportunities = (~allPositionAtBoard & movedPositions) | ((~ourPositions & attackPositions) & opponentPositionAtBoard);
                return opportunities;
            }
        }

        
    }
}

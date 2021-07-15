using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class Pawns : BasePiece
    {
        public const int MovingDirection = 8;
        public readonly int[] AttacksDirection = new int[2] { 7, 9 };

        //these mask we need if our pawn wants to attack and its at the sides, so therefore it cannot move onto the edge and jumping to another row
        public const ulong maskNotAColumn = 0b_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111_0111_1111;
        public const ulong maskNotHColumn = 0b_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110_1111_1110;
        //this mask checks if pawns at the starting position, and if they are, they got the chance to move 2 square
        public readonly ulong MaskOfDoubleMove;

        public Pawns(ColorSide color, ulong positions) : base(color, positions)
        {
            MaskOfDoubleMove = color == ColorSide.Black ? (ulong)0b_0000_0000_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000 : 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_0000_0000; 
        }
         

        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            if ((currentPosition & Positions) < 0) throw new Exception("selected positions not in the current positions");

            if(this.Color == ColorSide.Black)
            {
                ulong attackPositions = ((currentPosition >> AttacksDirection[0]) & maskNotAColumn) ^ ((currentPosition >> AttacksDirection[1]) & maskNotHColumn);
                //this one checks if we want to move double square and noone is in front of us
                ulong movedFirstPositions = (((MaskOfDoubleMove & currentPosition) >> 8) & ~allPositionAtBoard) >> 8;
                ulong movedPositions = (currentPosition >> MovingDirection) | movedFirstPositions;
                ulong opportunities = (~allPositionAtBoard & movedPositions) | ((~ourPositions & attackPositions) & opponentPositionAtBoard);
                return opportunities;
            }
            else
            {
                ulong attackPositions = ((currentPosition << AttacksDirection[0]) & maskNotAColumn) ^ ((currentPosition <<  AttacksDirection[1]) & maskNotHColumn);
                //this one checks if we want to move double square and noone is in front of us
                ulong movedFirstPositions = (((MaskOfDoubleMove & currentPosition) << 8) & ~allPositionAtBoard) << 8;
                ulong movedPositions = (currentPosition << MovingDirection) | movedFirstPositions;
                ulong opportunities = (~allPositionAtBoard & movedPositions) | ((~ourPositions & attackPositions) & opponentPositionAtBoard);
                
                //#region prints
                //PrintBoard(Convert.ToString((long)currentPosition, toBase: 2).PadLeft(64, '0'));
                //Console.WriteLine(" ");
                //PrintBoard(Convert.ToString((long)opponentPositionAtBoard, toBase: 2).PadLeft(64, '0'));
                //Console.WriteLine(" ");
                //PrintBoard(Convert.ToString((long)attackPositions, toBase: 2).PadLeft(64, '0'));
                //Console.WriteLine(" ");
                //PrintBoard(Convert.ToString((long)movedFirstPositions, toBase: 2).PadLeft(64, '0'));
                //Console.WriteLine(" ");
                //PrintBoard(Convert.ToString((long)movedPositions, toBase: 2).PadLeft(64, '0'));
                //Console.WriteLine(" ");
                //PrintBoard(Convert.ToString((long)opportunities, toBase: 2).PadLeft(64, '0'));
                //#endregion
                return opportunities;
            }


        }

    }
}

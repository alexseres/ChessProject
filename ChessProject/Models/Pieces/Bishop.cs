using ChessProject.ActionLogics.Attacks;
using ChessProject.Utils.BitScanLogic;
using ChessProject.Actions.Movements;
using ChessProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace ChessProject.Models.Pieces
{
    public class Bishop : BasePiece
    {
        public readonly string Image;
        public ILongMovements Movements { get; set; }
        public IBitScan BitScan { get; set; }
        public IAttack Attack { get; set; }

        private const int EastNorthDirection = 7;
        private const int WestNorthDirection = 9;
        private const int EastSouthDirection = -9;
        private const int WestSouthDirection = -7;

        public Bishop(Player player, ColorSide color, ulong position, IBitScan bitScan, ILongMovements movements, IAttack attack, string imagePath) : base(player, color, position, imagePath)
        {
            PType = PieceType.Bishop;
            Movements = movements;
            BitScan = bitScan;
            Attack = attack;
        }
        public override ulong Search(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            int square = BitScan.bitScanForwardLS1B(currentPosition);
            ulong eastNorthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetEastNorth, BitScan.bitScanForwardLS1B, EastNorthDirection);
            //Console.WriteLine(" ");
            //PrintBoard(Convert.ToString((long)eastNorthAttack, toBase: 2).PadLeft(64, '0'));
            //Console.WriteLine(" ");
            ulong westNorthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetWestNorth, BitScan.bitScanForwardLS1B, WestNorthDirection);
            //PrintBoard(Convert.ToString((long)westNorthAttack, toBase: 2).PadLeft(64, '0'));
            //Console.WriteLine(" ");
            ulong eastSouthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetEastSouth, BitScan.bitScanReverseMS1B, EastSouthDirection);

            //PrintBoard(Convert.ToString((long)eastSouthAttack, toBase: 2).PadLeft(64, '0'));
            //Console.WriteLine(" ");
            ulong westSouthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetWestSouth, BitScan.bitScanReverseMS1B, WestSouthDirection);
            //PrintBoard(Convert.ToString((long)westSouthAttack, toBase: 2).PadLeft(64, '0'));

            return eastNorthAttack ^ westNorthAttack ^ eastSouthAttack ^ westSouthAttack;
        }

        public override ulong GetSpecificAttackFromSearch(ulong currentPosition, ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions, ulong opponentPiecePosition)
        {
            ulong[] allMoves = new ulong[4];
            int square = BitScan.bitScanForwardLS1B(currentPosition);
            ulong eastNorthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetEastNorth, BitScan.bitScanForwardLS1B, EastNorthDirection);
            ulong westNorthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetWestNorth, BitScan.bitScanForwardLS1B, WestNorthDirection);
            ulong eastSouthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetEastSouth, BitScan.bitScanReverseMS1B, EastSouthDirection);
            ulong westSouthAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetWestSouth, BitScan.bitScanReverseMS1B, WestSouthDirection);
            //Debug.WriteLine("actual pos");
            //PrintBoard(Convert.ToString((long)currentPosition, toBase: 2).PadLeft(64, '0'));
            //PrintBoard(Convert.ToString((long)eastNorthAttack, toBase: 2).PadLeft(64, '0'));
            //PrintBoard(Convert.ToString((long)westNorthAttack, toBase: 2).PadLeft(64, '0'));
            //PrintBoard(Convert.ToString((long)eastSouthAttack, toBase: 2).PadLeft(64, '0'));
            //PrintBoard(Convert.ToString((long)westSouthAttack, toBase: 2).PadLeft(64, '0'));
            //PrintBoard(Convert.ToString((long)opponentPositionAtBoard, toBase: 2).PadLeft(64, '0'));
            //PrintBoard(Convert.ToString((long)opponentPiecePosition, toBase: 2).PadLeft(64, '0'));
            allMoves[0] = eastNorthAttack;
            allMoves[1] = westNorthAttack;
            allMoves[2] = eastSouthAttack;
            allMoves[3] = westSouthAttack;

            foreach (ulong moves in allMoves)
            {
                if ((moves & opponentPiecePosition) > 0)
                {
                    return moves;
                }
            }
            return 0;
        }

        public void PrintBoard(string board)
        {
            Debug.WriteLine("Bishop");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < board.Length; i++)
            {
                if (i % 8 == 0 && i != 0)
                {
                    string row = new string(sb.ToString());
                    Debug.WriteLine(row);
                    sb.Clear();
                }
                sb.Append(board[i]);
            }
            var finalRow = new string(sb.ToString());
            Debug.WriteLine(finalRow);
            Debug.WriteLine(" ");
        }
    }
}

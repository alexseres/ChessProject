using ChessProject.ActionLogics.Attacks;
using ChessProject.Actions.Movements;
using ChessProject.Models.Enums;
using ChessProject.Utils.BitScanLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.Models.Pieces
{
    public class Rook : BasePiece
    {
        public IBitScan BitScan { get; set; }
        public ILongMovements Movements { get; set; }
        public IAttack Attack { get; set; }
        private const int NorthDirection = 8;
        private const int EastDiretion = -1;
        private const int SouthDirection = -8;
        private const int WestDirection = 1;

        public Rook(Player player, ColorSide color, ulong position, IBitScan bitScan, ILongMovements movements, IAttack attack, string imagePath) : base(player, color, position, imagePath)
        {
            PType = PieceType.Rook;
            BitScan = bitScan;
            Movements = movements;
            Attack = attack;
        }

        public override ulong Search(ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions)
        {
            int square = BitScan.bitScanForwardLS1B(this.Position);
            ulong northAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetNorth, BitScan.bitScanForwardLS1B, NorthDirection);
            ulong eastAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetEast, BitScan.bitScanReverseMS1B, EastDiretion);
            ulong southAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetSouth, BitScan.bitScanReverseMS1B, SouthDirection);
            ulong westAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetWest, BitScan.bitScanForwardLS1B, WestDirection);
            return northAttack ^ eastAttack ^ southAttack ^ westAttack;
        }

        public override ulong GetSpecificAttackFromSearch(ulong allPositionAtBoard, ulong opponentPositionAtBoard, ulong ourPositions, ulong opponentPiecePosition)
        {
            ulong[] allMoves = new ulong[8];
            int square = BitScan.bitScanForwardLS1B(this.Position);
            ulong northAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetNorth, BitScan.bitScanForwardLS1B, NorthDirection);
            ulong eastAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetEast, BitScan.bitScanReverseMS1B, EastDiretion);
            ulong southAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetSouth, BitScan.bitScanReverseMS1B, SouthDirection);
            ulong westAttack = Attack.GetRayAttacks(allPositionAtBoard, opponentPositionAtBoard, square, Movements.GetWest, BitScan.bitScanForwardLS1B, WestDirection);
            allMoves[0] = northAttack;
            allMoves[1] = eastAttack;
            allMoves[2] = southAttack;
            allMoves[3] = westAttack;
            foreach (ulong moves in allMoves)
            {
                if ((moves & opponentPiecePosition) > 0)
                {
                    return moves;
                }
            }
            return 0;
        }
    }
}

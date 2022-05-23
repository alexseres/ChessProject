using ChessProject.Models;
using ChessProject.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChessProject.ServiceLayers
{
    public interface IMainGameService
    {
        public Player NextPlayer { get; set; }
        public void PawnSwapperLogicForPlayer1(BasePiece piece);
        public void PawnSwapperLogicForPlayer2(BasePiece piece);
        public (bool, string) CheckIfIsThreeFoldOrFiftyMove(Player player);
        public string IsPlayerInCheckAndCheckmateChecker(Player actualPlayer, Player opponent);
        public (bool, string) ProcessOfMakingSurePlayerCanChooseSpecificPiece(Player player, BasePiece piece);
        public (bool,string) ProcessOfMakingSurePlayerCanDropSpecificPiece(Player player, BasePiece piece, ulong currentPiecePosition, ulong opportunities, ulong choosenPositionToMove);
    }
}

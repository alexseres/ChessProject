using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class UpdateBitBoards : IUpdateBitBoards
    {
        public void UpdateAllBitBoard(bool attacked, Player currentPlayer, Player opponent, ulong choosenPositionToMove, ulong opportunities, ulong currentPosition, ulong boardWithAllMember)
        {
            if(attacked)
            {
                opponent.NotifyBeingAttacked(choosenPositionToMove);
            }
            currentPlayer.NotifyMove(currentPosition, opportunities, choosenPositionToMove);
            boardWithAllMember = currentPlayer.Pieces ^ opponent.Pieces;
        }

        public ulong[] SeparateUpdateBitBoardsToEvadeCheck(ulong newPositionOfDefender, ulong oldPositionOfDefender, ulong defenderPieceRoute, ulong allPiecePositions, ulong ourPositions, ulong opponentPositions)
        {
            ulong[] positions = new ulong[3];
            if ((opponentPositions & newPositionOfDefender) > 0) opponentPositions = opponentPositions & ~newPositionOfDefender;
            ourPositions = (ourPositions & ~oldPositionOfDefender) ^ newPositionOfDefender;
            allPiecePositions = ourPositions ^ opponentPositions;
            positions[0] = allPiecePositions; positions[1] = opponentPositions; positions[2] = ourPositions;
            return positions;
        }

        public List<IObserver> SeparateUpdatePieceList(List<IObserver> opponentList, ulong occupiedPos)
        {
            List<IObserver> copyOfOpponentList = opponentList;
            foreach(IObserver observer in copyOfOpponentList)
            {
                BasePiece piece = observer as BasePiece;
                
                if((piece.Positions & occupiedPos) > 0)
                {
                    piece.Positions = (piece.Positions & ~occupiedPos);
                }
            }
            return copyOfOpponentList;
        }
    }
}

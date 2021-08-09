using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class UpdateBitBoards : IUpdateBitBoards
    {

        public void PrintBoard(string board)
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
            var finalRow = new string(sb.ToString());
            Console.WriteLine(finalRow);
        }

        public void UpdateAllBitBoard(bool attacked, Player currentPlayer, Player opponent, ulong choosenPositionToMove, ulong opportunities, ulong currentPosition, ref ulong BoardWithAllMember)
        {
            if(attacked)
            {
                opponent.NotifyBeingAttacked(choosenPositionToMove);
            }
            currentPlayer.NotifyMove(currentPosition, opportunities, choosenPositionToMove);
            BoardWithAllMember = currentPlayer.PiecesPosition ^ opponent.PiecesPosition;
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
                
                if((piece.Position & occupiedPos) > 0)
                {
                    piece.Position = (piece.Position & ~occupiedPos);
                }
            }
            return copyOfOpponentList;
        }
    }
}

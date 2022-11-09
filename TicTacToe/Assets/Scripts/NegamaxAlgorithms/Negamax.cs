using System;

namespace NegamaxAlgorithms
{
    public class Negamax : INegamax
    {
        private string AILetter;

        private string PlayerLetter;

        public Negamax(string aiLetter, string playerLetter)
        {
            AILetter = aiLetter;
            PlayerLetter = playerLetter;
        }


        public Move FindBestTurn(Board board)
        {
            int bestVal = -1000;
            Move bestMove = new Move();
            bestMove.row = -1;
            bestMove.col = -1;
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Check if cell is empty
                    if (board[i, j] == "")
                    {
                        board[i, j] = AILetter;
                        
                        int moveVal = -1*EvaluateBoard(board, 1, -1);
 
                       
                        board[i, j] = "";
                        
                        if (moveVal > bestVal)
                        {
                            bestMove.row = i;
                            bestMove.col = j;
                            bestVal = moveVal;
                        }
                    }
                }
            }

            return bestMove;
        }

        private static bool HasMovesLeft(Board board)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i,j]=="")
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private int EvaluateBoard(Board board, int depth, int color)
        {
            if (depth == 9 || GameIsFinished(board))
            {
                return color * Evaluate(board, depth);
            }

            int value = int.MinValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == "")
                    {
                        board[i, j] = color==1?AILetter:PlayerLetter;
                        
                        value = Math.Max(value, -1*EvaluateBoard(board,
                            depth + 1, color*-1));
                        
                        board[i, j] = "";
                    }
                }
            }

            return value;
        }

        private static bool GameIsFinished(Board board)
        {
            return !HasMovesLeft(board) || CheckForWin(board, out _);
        }

        private int Evaluate(Board board, int depth)
        {
            if (!CheckForWin(board, out string player))
            {
                return 0;
            }

            if (player == AILetter)
            {
                return 1000 / depth;
            }
            else
            {
                return -1000 / depth;
            }

        }

        private static bool CheckForWin(Board board, out string player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i,0]==board[i,1] && board[i,1]==board[i,2] && board[i,0]!="")
                {
                    player = board[i, 0];
                    return true;
                }
            }
            
            for (int i = 0; i < 3; i++)
            {
                if (board[0,i]==board[1,i] && board[1,i]==board[2,i] && board[0,i]!="")
                {
                    player = board[0, i];
                    return true;
                }
            }

            if (board[0,0]==board[1,1] && board[1,1]==board[2,2] && board[0,0]!="")
            {
                player = board[0, 0];
                return true;
            }
            
            if (board[0,2]==board[1,1] && board[1,1]==board[2,0] && board[0,2]!="")
            {
                player = board[0, 2];
                return true;
            }

            player = "";
            return false;
        }
    }

    public class Move
    {
        public int row;
        public int col;
    }
}
using System;

namespace NegamaxAlgorithms
{
    public class Negamax : INegamax
    {
        public string ComputerSide { get; set; }
        public string PlayerSide { get; set; }

        public Move FindBestTurn(Board board)
        {
            int bestVal = -1000;
            Move bestMove = new Move
            {
                Row = -1,
                Col = -1
            };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == "")
                    {
                        board[i, j] = ComputerSide;

                        int moveVal = -1 * EvaluateBoard(board, 1, -1);


                        board[i, j] = "";

                        if (moveVal > bestVal)
                        {
                            bestMove.Row = i;
                            bestMove.Col = j;
                            bestVal = moveVal;
                        }
                    }
                }
            }

            return bestMove;
        }


        private int EvaluateBoard(Board board, int depth, int color)
        {
            if (depth == 9 || board.State != Board.GameState.NotFinished)
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
                        board[i, j] = color == 1 ? ComputerSide : PlayerSide;

                        value = Math.Max(value, -1 * EvaluateBoard(board,
                            depth + 1, color * -1));

                        board[i, j] = "";
                    }
                }
            }

            return value;
        }

        private int Evaluate(Board board, int depth)
        {
            if (!board.CheckForWin(out string player))
            {
                return 0;
            }

            if (player == ComputerSide)
            {
                return 1000 / depth;
            }
            else
            {
                return -1000 / depth;
            }
        }
    }
}
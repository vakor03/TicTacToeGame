using System;
using TMPro;

namespace NegamaxAlgorithms
{
    public class Board
    {
        public string this[int i, int j]
        {
            get => _board[i, j];
            set => _board[i, j] = value;
        }
        private string[,] _board;

        public Board()
        {
            _board = new string[3, 3];
            Clear();
        }

        public void Clear()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _board[i,j]=String.Empty;
                }
            }
        }

        public static implicit operator Board(TMP_Text[] tmpTexts)
        {
            Board board = new Board();
            board._board = new string[3, 3];
            for (int i = 0; i < tmpTexts.Length; i++)
            {
                board[i / 3, i % 3] = tmpTexts[i].text;
            }

            return board;
        }


        public GameState State
        {
            get
            {
                if (CheckForWin(out _))
                {
                    return GameState.Win;
                }

                if (!HasMovesLeft())
                {
                    return GameState.Draw;
                }

                return GameState.NotFinished;
            }
        }
        
        private bool HasMovesLeft()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (_board[i,j]=="")
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool CheckForWin(out string player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (_board[i,0]==_board[i,1] && _board[i,1]==_board[i,2] && _board[i,0]!="")
                {
                    player = _board[i, 0];
                    return true;
                }
            }
            
            for (int i = 0; i < 3; i++)
            {
                if (_board[0,i]==_board[1,i] && _board[1,i]==_board[2,i] && _board[0,i]!="")
                {
                    player = _board[0, i];
                    return true;
                }
            }

            if (_board[0,0]==_board[1,1] && _board[1,1]==_board[2,2] && _board[0,0]!="")
            {
                player = _board[0, 0];
                return true;
            }
            
            if (_board[0,2]==_board[1,1] && _board[1,1]==_board[2,0] && _board[0,2]!="")
            {
                player = _board[0, 2];
                return true;
            }

            player = "";
            return false;
        }

        public enum GameState
        {
            NotFinished,
            Draw,
            Win
        }
    }
}
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
    }
}
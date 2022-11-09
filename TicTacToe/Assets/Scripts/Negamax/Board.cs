namespace Negamax
{
    public class Board
    {
        public string this[int i, int j]
        {
            get => _board[i, j];
            set => _board[i, j] = value;
        }
        private string[,] _board;
        
        
    }
}
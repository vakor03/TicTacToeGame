namespace NegamaxAlgorithms
{
    public interface INegamax
    {
        Move FindBestTurn(Board board);
        string ComputerSide { get; set; }
        string PlayerSide { get; set; }
    }
}
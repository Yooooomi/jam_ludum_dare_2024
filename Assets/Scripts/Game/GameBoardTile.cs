public class GameBoardTile
{
  public GameCard card;

  public bool HoldsCard()
  {
    return card != null;
  }
}
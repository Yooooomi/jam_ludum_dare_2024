public class GameBoardTile
{
  public GameCard card;
  public int x;
  public int y;

  public GameBoardTile(int x, int y)
  {
    this.x = x;
    this.y = y;
  }

  public bool HoldsCard()
  {
    return card != null;
  }
}
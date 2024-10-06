public class HasteOfTheWilds : GameCard
{
  public HasteOfTheWilds()
  {
    GameBridge.instance.onPlaced.AddListener(OnPlaced);
  }

  private void OnPlaced(GameCard placed)
  {
    var down = board.GetDown(this);

    if (down == null)
    {
      return;
    }
    down.Age(1);
  }
}
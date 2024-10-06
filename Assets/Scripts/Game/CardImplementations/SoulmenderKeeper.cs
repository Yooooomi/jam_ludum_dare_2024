public class SoulmenderKeeper : GameCard
{
  public SoulmenderKeeper()
  {
    GameBridge.instance.onPlaced.AddListener(OnPlaced);
  }

  private void OnPlaced(GameCard placed)
  {
    if (!IsSelf(placed))
    {
      return;
    }
    var cards = board.GetLine(this);
    foreach (var card in cards)
    {
      if (card == this)
      {
        continue;
      }
      card.Heal(2);
    }
  }
}

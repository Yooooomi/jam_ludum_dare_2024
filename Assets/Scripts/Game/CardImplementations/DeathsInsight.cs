public class DeathsInsight : GameCard
{
  public DeathsInsight()
  {
    GameBridge.instance.onKilled.AddListener(OnKilled);
  }

  private void OnKilled(GameCard victim, GameCard from)
  {
    var down = board.GetDown(this);
    if (down != from)
    {
      return;
    }
    GameState.instance.GetPlayer(from.playerId).DrawCards(1);
  }
}
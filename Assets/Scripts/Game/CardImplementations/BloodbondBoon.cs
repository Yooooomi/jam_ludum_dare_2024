public class BloodbondBoon : GameCard
{
  public BloodbondBoon()
  {
    GameBridge.instance.onKilled.AddListener(OnKilled);
  }

  private void OnKilled(GameCard victim, GameCard from)
  {
    var up = board.GetUp(this);
    if (up != from)
    {
      return;
    }
    GameState.instance.GetPlayer(from.playerId).RewardMana(1);
  }
}
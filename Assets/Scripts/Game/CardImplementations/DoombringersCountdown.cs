public class DoombringersCountdown : GameCard
{
  public DoombringersCountdown()
  {
    GameBridge.instance.onTurnEnd.AddListener(OnTurnEnd);
  }

  public override void Age(int amount)
  {
    // Cannot age doomsbringer
  }

  private void OnTurnEnd(int playerIdTurn)
  {
    if (playerId != playerIdTurn)
    {
      return;
    }
    if (lifetime != 2)
    {
      return;
    }
    GameState.instance.GetOtherPlayer(playerId).GetAttacked(10);
    LoseHealth(100, this);
  }
}
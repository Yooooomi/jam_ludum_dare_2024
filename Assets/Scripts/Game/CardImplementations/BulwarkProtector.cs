using System.Collections.Generic;

public class BulwarkProtector : GameCard
{
  public BulwarkProtector()
  {
    GameBridge.instance.onPlaced.AddListener(OnPlaced);
    GameBridge.instance.onKilled.AddListener(OnKilled);
  }

  private readonly List<GameCard> buffed = new();

  private int HealthAbsorber(int amount)
  {
    var killed = LoseHealth(amount, this);
    if (killed)
    {
      return -stats.health;
    }
    return amount;
  }

  private void ComputeHealthAbsorber()
  {
    var cards = new List<GameCard>() { board.GetLeft(this), board.GetRight(this), board.GetDown(this), board.GetUp(this), };
    foreach (var card in cards)
    {
      if (card == null)
      {
        continue;
      }
      if (card.HasHealthAbsorber(HealthAbsorber))
      {
        continue;
      }
      card.RegisterHealthAbsorber(HealthAbsorber);
      buffed.Add(card);
    }
  }

  private void RemoveHealthAbsorber()
  {
    foreach (var card in buffed)
    {
      card.RemoveHealthAbsorber(HealthAbsorber);
    }
  }

  private void OnPlaced(GameCard placed)
  {
    ComputeHealthAbsorber();
  }

  private void OnKilled(GameCard killed, GameCard from)
  {
    if (!IsSelf(killed))
    {
      return;
    }
    RemoveHealthAbsorber();
  }
}

using System.Collections.Generic;

public abstract class PerTurnCardBuffer : GameCard
{
  public PerTurnCardBuffer()
  {
    GameBridge.instance.onTurnBegin.AddListener(OnTurnBegin);
  }

  protected List<GameCard> LeftRight()
  {
    return new List<GameCard>() { board.GetLeft(this), board.GetRight(this) };
  }

  protected List<GameCard> Cross()
  {
    return new List<GameCard>() { board.GetLeft(this), board.GetRight(this), board.GetUp(this), board.GetDown(this) };
  }

  protected List<GameCard> Left()
  {
    return new List<GameCard>() { board.GetLeft(this) };
  }

  protected List<GameCard> Right()
  {
    return new List<GameCard>() { board.GetRight(this) };
  }

  protected abstract List<GameCard> GetCardsToBuff();
  protected abstract GameCardStats Modifier(GameCardStats stats, GameCardStats rawStats);
  protected virtual void OnApplied(GameCard to) { }

  private void ApplyBuff(GameCard to)
  {
    if (to.HasStatModifier(Modifier))
    {
      return;
    }
    to.RegisterStatModifier(Modifier);
    OnApplied(to);
  }

  private void ComputeBuffs()
  {
    var toApply = GetCardsToBuff();
    foreach (var to in toApply)
    {
      if (to == null)
      {
        continue;
      }
      ApplyBuff(to);
    }
  }

  private void OnTurnBegin(int playerId)
  {
    if (this.playerId != playerId)
    {
      return;
    }
    ComputeBuffs();
  }
}
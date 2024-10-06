using System.Collections.Generic;
using UnityEngine;

public abstract class CardBuffer : GameCard
{
  public CardBuffer()
  {
    GameBridge.instance.onPlaced.AddListener(OnPlaced);
    GameBridge.instance.onKilled.AddListener(OnKilled);
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

  private List<GameCard> buffed = new();

  private void ApplyBuff(GameCard to)
  {
    if (to.HasStatModifier(Modifier))
    {
      return;
    }
    to.RegisterStatModifier(Modifier);
    buffed.Add(to);
    OnApplied(to);
  }

  private void RemoveBuffs()
  {
    foreach (var buff in buffed)
    {
      buff.RemoveStatModifier(Modifier);
    }
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

  private void OnPlaced(GameCard placed)
  {
    ComputeBuffs();
  }

  private void OnKilled(GameCard killed)
  {
    if (!IsSelf(killed))
    {
      return;
    }
    RemoveBuffs();
  }
}
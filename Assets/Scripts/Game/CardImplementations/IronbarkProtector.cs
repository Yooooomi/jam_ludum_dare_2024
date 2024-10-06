using System.Collections.Generic;

public class IronbarkProtector : CardBuffer
{
  protected override List<GameCard> GetCardsToBuff()
  {
    return LeftRight();
  }

  protected override GameCardStats Modifier(GameCardStats stats, GameCardStats rawStats)
  {
    stats.maxHealth += 2;
    return stats;
  }

  protected override void OnApplied(GameCard to)
  {
    base.OnApplied(to);
    to.Heal(2);
  }
}

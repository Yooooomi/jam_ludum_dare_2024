using System.Collections.Generic;

public class LifetenderSentinel : CardBuffer
{
  protected override List<GameCard> GetCardsToBuff()
  {
    return Cross();
  }

  protected override GameCardStats Modifier(GameCardStats stats, GameCardStats rawStats)
  {
    stats.maxHealth += 1;
    return stats;
  }

  protected override void OnApplied(GameCard to)
  {
    base.OnApplied(to);
    to.Heal(1);
  }
}

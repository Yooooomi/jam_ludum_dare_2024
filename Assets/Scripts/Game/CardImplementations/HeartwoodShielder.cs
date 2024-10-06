using System.Collections.Generic;

public class HeartwoodShielder : PerTurnCardBuffer
{
  protected override List<GameCard> GetCardsToBuff()
  {
    return Left();
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

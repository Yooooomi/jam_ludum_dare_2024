using System.Collections.Generic;

public class BladeleafWatcher : PerTurnCardBuffer
{
  protected override List<GameCard> GetCardsToBuff()
  {
    return Left();
  }

  protected override GameCardStats Modifier(GameCardStats stats, GameCardStats rawStats)
  {
    stats.damage += 1;
    return stats;
  }
}

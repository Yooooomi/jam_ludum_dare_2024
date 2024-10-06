using System.Collections.Generic;

public class MightGuardian : CardBuffer
{
  protected override GameCardStats Modifier(GameCardStats stats, GameCardStats rawStats)
  {
    stats.damage += 2;
    return stats;
  }

  protected override List<GameCard> GetCardsToBuff()
  {
    return LeftRight();
  }
}

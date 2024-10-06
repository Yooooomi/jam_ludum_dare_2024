using System.Collections.Generic;

public class ValorWarden : CardBuffer
{
  protected override GameCardStats Modifier(GameCardStats stats, GameCardStats rawStats)
  {
    stats.damage += 1;
    return stats;
  }

  protected override List<GameCard> GetCardsToBuff()
  {
    return Cross();
  }
}

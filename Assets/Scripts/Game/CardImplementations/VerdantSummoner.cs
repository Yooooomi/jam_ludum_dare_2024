public class VerdantSummoner : GameCard
{
  public VerdantSummoner()
  {
    GameBridge.instance.onPlaced.AddListener(OnPlaced);
  }

  private GameCardStats Modifier(GameCardStats stats, GameCardStats rawStats)
  {
    stats.maxHealth += rawStats.maxHealth;
    stats.damage += rawStats.damage;
    return stats;
  }

  private void OnPlaced(GameCard placed)
  {
    var right = board.GetRight(this);
    if (right != placed)
    {
      return;
    }
    placed.RegisterStatModifier(Modifier);
    placed.Heal(placed.GetBaseStats().health);
  }
}

public class Roger : GameCard
{
    public Roger()
    {
        GameBridge.instance.onPlaced.AddListener(OnPlaced);
    }

    private GameCardStats Modifier(GameCardStats stats)
    {
        stats.maxHealth += 2;
        return stats;
    }


    private void OnPlaced(GameCard card)
    {
        if (!IsSelf(card))
        {
            return;
        }
        var left = GameState.instance.GetPlayer(playerId).board.GetLeft(card);
        if (left == null)
        {
            return;
        }
        left.RegisterStatModifier(Modifier);
        left.Heal(2);
    }

    private void OnKilled(GameCard card)
    {
        if (!IsSelf(card))
        {
            return;
        }
        var left = GameState.instance.GetPlayer(playerId).board.GetLeft(card);
        if (left == null)
        {
            return;
        }
        left.RemoveStatModifier(Modifier);
    }
}

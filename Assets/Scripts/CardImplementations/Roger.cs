
public class Roger : CardBehavior
{
    private CardStats Modifier(CardStats stats)
    {
        stats.maxHealth += 2;
        return stats;
    }

    protected override void OnPlaced(CardBehavior card)
    {
        base.OnPlaced(card);
        if (!IsSelf(card))
        {
            return;
        }
        var left = board.GetLeft(card);
        if (left == null)
        {
            return;
        }
        left.RegisterStatModifier(Modifier);
        left.Heal(2);
    }

    protected override void OnKilled(CardBehavior card)
    {
        base.OnKilled(card);
        if (!IsSelf(card))
        {
            return;
        }
        var left = board.GetLeft(card);
        if (left == null)
        {
            return;
        }
        left.RemoveStatModifier(Modifier);
    }
}

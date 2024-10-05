public class TinyCreature : CardBehavior
{
    protected override void OnAttack(CardBehavior target)
    {
        base.OnAttack(target);
        target.LoseHealth(stats.damage);
    }
}

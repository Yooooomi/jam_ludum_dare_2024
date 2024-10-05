public class TinyCreature : CardBehavior
{
    protected override void OnAttack(Card target)
    {
        base.OnAttack(target);
        target.Attack(card.damage);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public struct CardStats
{
    public int maxHealth;
    public int damage;
    public int health;
    public int mana;
}

public abstract class CardBehavior : MonoBehaviour
{
    public int playerId;
    public string cardName;
    public CardStats stats;
    protected PlayerBoard board;
    private int lifetime;
    private int lastLifetimeAttack;

    private readonly List<Func<CardStats, CardStats>> modifiers = new();

    private void ComputeCardStats()
    {
        CardStats cloned = stats;
        foreach (var modifier in modifiers)
        {
            cloned = modifier(cloned);
        }
        stats = cloned;
        if (stats.health > stats.maxHealth)
        {
            stats.health = stats.maxHealth;
        }
    }

    public void RegisterStatModifier(Func<CardStats, CardStats> modifier)
    {
        modifiers.Add(modifier);
        ComputeCardStats();
    }

    public void RemoveStatModifier(Func<CardStats, CardStats> modifier)
    {
        modifiers.Remove(modifier);
        ComputeCardStats();
    }

    public bool Attack(CardBehavior target)
    {
        var killed = target.LoseHealth(stats.damage);
        lastLifetimeAttack = lifetime;
        return killed;
    }

    public bool LoseHealth(int damage)
    {
        stats.health = Mathf.Clamp(stats.health - damage, 0, stats.maxHealth);
        Destroy(gameObject);
        GameState.instance.SendMessage("OnKilled", this);
        return stats.health < 0;
    }

    public void Heal(int heal)
    {
        stats.health = Mathf.Clamp(stats.health + heal, 0, stats.maxHealth);
    }


    protected bool IsSelf(CardBehavior card)
    {
        return this == card;
    }

    protected virtual void OnPlaced(CardBehavior card)
    {
        Debug.Log("OnPlaced");
    }

    protected virtual void OnKilled(CardBehavior card)
    {
        Debug.Log("OnKilled");
    }

    protected virtual void OnTurnBegin(int playerId)
    {
        if (this.playerId != playerId)
        {
            return;
        }
        lifetime += 1;
        Debug.Log("OnTurnBegin");
    }

    protected virtual void OnTurnEnd()
    {
        Debug.Log("OnTurnEnd");
    }

    protected virtual void OnDamageTaken(CardBehavior from)
    {
        Debug.Log("OnDamageTaken");
    }

    protected virtual void OnAttack(CardBehavior target)
    {
        Debug.Log("OnAttack");
    }

    public virtual bool CanAttack()
    {
        return lifetime > 0;
    }
}

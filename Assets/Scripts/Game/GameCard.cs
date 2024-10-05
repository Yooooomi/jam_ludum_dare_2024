using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GameCardStats
{
    public int maxHealth;
    public int health;
    public int mana;
    public int damage;
}

public class GameCard
{
    private readonly List<Func<GameCardStats, GameCardStats>> modifiers = new();
    private GameCardStats stats;

    public int playerId;
    private int lifetime;
    private int lastLifetimeAttack;
    public CardInfo info;

    public GameCard(GameCardStats stats, CardInfo info)
    {
        GameBridge.instance.onTurnBegin.AddListener(OnTurnBegin);
        this.stats = stats;
        this.info = info;
    }

    protected bool IsSelf(GameCard other)
    {
        return this == other;
    }

    private void OnTurnBegin(int playerId)
    {
        if (this.playerId != playerId)
        {
            return;
        }
        lifetime += 1;
    }

    public GameCardStats GetCardStats()
    {
        return ComputeCardStats();
    }

    private GameCardStats ComputeCardStats()
    {
        GameCardStats cloned = stats;
        foreach (var modifier in modifiers)
        {
            cloned = modifier(cloned);
        }
        if (cloned.health > cloned.maxHealth)
        {
            cloned.health = cloned.maxHealth;
        }
        return cloned;
    }

    public bool LoseHealth(int damage)
    {
        stats.health -= damage;
        GameBridge.instance.onDamageTaken.Invoke(this);
        return stats.health <= damage;
    }

    public bool Attack(GameCard target)
    {
        GameBridge.instance.onAttack.Invoke(this);
        var killed = target.LoseHealth(ComputeCardStats().damage);
        return killed;
    }

    public void Heal(int amount)
    {
        stats.health = Mathf.Clamp(stats.health + amount, 0, stats.maxHealth);
    }

    public bool CanAttack()
    {
        return lifetime > 0 && lastLifetimeAttack != lifetime;
    }

    public void RegisterStatModifier(Func<GameCardStats, GameCardStats> modifier)
    {
        modifiers.Add(modifier);
        GameBridge.instance.onCardUpdate.Invoke(this);
    }

    public void RemoveStatModifier(Func<GameCardStats, GameCardStats> modifier)
    {
        modifiers.Remove(modifier);
        GameBridge.instance.onCardUpdate.Invoke(this);
    }
}

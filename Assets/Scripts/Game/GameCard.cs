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

    public GameCardStats Difference(GameCardStats other)
    {
        return new GameCardStats
        {
            maxHealth = maxHealth - other.maxHealth,
            health = health - other.health,
            mana = mana - other.mana,
            damage = damage - other.damage,
        };
    }
}

public class GameCard
{
    private readonly List<Func<GameCardStats, GameCardStats, GameCardStats>> modifiers = new();
    private readonly List<Func<int, int>> healthAbsorbers = new();

    protected GameCardStats stats;

    public int playerId;
    private int lifetime;
    private int lastLifetimeAttack;
    public CardInfo info;
    public GameBoard board;

    public GameCard()
    {
        GameBridge.instance.onTurnBegin.AddListener(OnTurnBegin);
    }

    public void Setup(int playerId, GameBoard playerBoard, GameCardStats stats, CardInfo info)
    {
        this.playerId = playerId;
        this.stats = stats;
        this.info = info;
        board = playerBoard;
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

    public GameCardStats GetBaseStats()
    {
        return stats;
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
            cloned = modifier(cloned, stats);
        }
        if (cloned.health > cloned.maxHealth)
        {
            cloned.health = cloned.maxHealth;
        }
        return cloned;
    }

    protected bool LoseHealth(int damage)
    {
        foreach (var healthAbsorber in healthAbsorbers)
        {
            damage -= healthAbsorber(damage);
        }
        if (damage <= 0)
        {
            return false;
        }
        var before = GetCardStats();
        stats.health -= damage;
        if (stats.health > 0)
        {
            GameBridge.instance.onCardStatChange.Invoke(this, GetCardStats().Difference(before));
        }
        else
        {
            GameBridge.instance.onKilled.Invoke(this);
        }
        return stats.health <= 0;
    }

    public bool GetAttacked(GameCard attacker)
    {
        var killed = LoseHealth(attacker.GetCardStats().damage);
        attacker.LoseHealth(GetCardStats().damage);
        return killed;
    }

    public bool Attack(GameCard target)
    {
        GameBridge.instance.onAttack.Invoke(this, target);
        var killed = target.GetAttacked(this);
        lastLifetimeAttack = lifetime;
        return killed;
    }

    public bool AttackHero(GamePlayer target)
    {
        GameBridge.instance.onHeroAttack.Invoke(this, target);
        target.GetAttacked(GetCardStats().damage);
        lastLifetimeAttack = lifetime;
        return false;
    }

    public void Heal(int amount)
    {
        var before = GetCardStats();
        stats.health = Mathf.Clamp(stats.health + amount, 0, before.maxHealth);
        GameBridge.instance.onCardStatChange.Invoke(this, GetCardStats().Difference(before));
    }

    private bool IsSpectator()
    {
        return info.cardType == CardType.Spectator;
    }

    public bool CanAttack()
    {
        // if (IsSpectator())
        // {
        //     return false;
        // }
        return lifetime > 0 && lastLifetimeAttack != lifetime;
    }

    public bool HasStatModifier(Func<GameCardStats, GameCardStats, GameCardStats> modifier)
    {
        return modifiers.Contains(modifier);
    }

    public void RegisterStatModifier(Func<GameCardStats, GameCardStats, GameCardStats> modifier)
    {
        // if (IsSpectator())
        // {
        //     return;
        // }
        var before = GetCardStats();
        modifiers.Add(modifier);
        GameBridge.instance.onCardStatChange.Invoke(this, GetCardStats().Difference(before));
    }

    public void RemoveStatModifier(Func<GameCardStats, GameCardStats, GameCardStats> modifier)
    {
        var before = GetCardStats();
        modifiers.Remove(modifier);
        GameBridge.instance.onCardStatChange.Invoke(this, GetCardStats().Difference(before));
    }

    public bool HasHealthAbsorber(Func<int, int> modifier)
    {
        return healthAbsorbers.Contains(modifier);
    }

    public void RegisterHealthAbsorber(Func<int, int> modifier)
    {
        // if (IsSpectator())
        // {
        //     return;
        // }
        healthAbsorbers.Add(modifier);
    }

    public void RemoveHealthAbsorber(Func<int, int> modifier)
    {
        healthAbsorbers.Remove(modifier);
    }

    public bool IsHealthBuffed()
    {
        return GetBaseStats().maxHealth < GetCardStats().maxHealth;
    }

    public bool IsDamageBuffed()
    {
        return GetBaseStats().damage < GetCardStats().damage;
    }
}

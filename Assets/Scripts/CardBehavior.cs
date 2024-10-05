using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
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
    [SerializeField]
    private CardStats stats;
    private CardStats mutableStats;
    protected PlayerBoard board;
    private int lifetime;
    private int lastLifetimeAttack;

    private readonly List<Func<CardStats, CardStats>> modifiers = new();

    public UnityEvent onStatChanged = new();

    private void Start()
    {
        GameState.instance.onPlaced.AddListener(OnPlaced);
    }

    private CardStats ComputeCardStats()
    {
        CardStats cloned = stats;
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

    public CardStats GetCardStats()
    {
        return ComputeCardStats();
    }

    public bool IsDamageBuffed()
    {
        var buffed = ComputeCardStats();
        return buffed.damage > stats.damage;
    }

    public bool IsHealthBuffed()
    {
        var buffed = ComputeCardStats();
        return buffed.maxHealth > stats.maxHealth;
    }

    public void RegisterStatModifier(Func<CardStats, CardStats> modifier)
    {
        modifiers.Add(modifier);
        onStatChanged.Invoke();
    }

    public void RemoveStatModifier(Func<CardStats, CardStats> modifier)
    {
        modifiers.Remove(modifier);
        onStatChanged.Invoke();
    }

    public bool Attack(CardBehavior target)
    {
        GameState.instance.onAttack.Invoke(this);
        var killed = target.LoseHealth(stats.damage);
        lastLifetimeAttack = lifetime;
        return killed;
    }

    public bool LoseHealth(int damage)
    {
        Debug.Log($"I am losing {damage} HP {playerId}");
        mutableStats.health = Mathf.Clamp(mutableStats.health - damage, 0, mutableStats.maxHealth);
        if (mutableStats.health <= 0)
        {
            GameState.instance.onKilled.Invoke(this);
            Destroy(gameObject);
        }
        else
        {
            GameState.instance.onDamageTaken.Invoke(this);
        }
        onStatChanged.Invoke();
        return mutableStats.health < 0;
    }

    public void Heal(int heal)
    {
        mutableStats.health = Mathf.Clamp(mutableStats.health + heal, 0, mutableStats.maxHealth);
        onStatChanged.Invoke();
    }

    protected bool IsSelf(CardBehavior card)
    {
        return this == card;
    }

    protected virtual void OnPlaced(CardBehavior card)
    {
        if (card != this)
        {
            return;
        }
        GameState.instance.onKilled.AddListener(OnKilled);
        GameState.instance.onTurnBegin.AddListener(OnTurnBegin);
        GameState.instance.onTurnEnd.AddListener(OnTurnEnd);
        GameState.instance.onDamageTaken.AddListener(OnDamageTaken);
        GameState.instance.onAttack.AddListener(OnAttack);
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

    protected virtual void OnTurnEnd(int playedId)
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
        return lifetime > 0 && lastLifetimeAttack != lifetime;
    }
}

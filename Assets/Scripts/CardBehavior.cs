using System.Collections.Generic;
using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    public GameCard card;

    private static readonly Dictionary<GameCard, CardBehavior> GameCardToCardBehavior = new();

    public static CardBehavior FromGameCard(GameCard source)
    {
        GameCardToCardBehavior.TryGetValue(source, out var found);
        return found;
    }

    private void Awake()
    {
        DelayedGameBridge.instance.onKilled.AddListener(OnKilled);
    }

    private void OnKilled(GameCard card)
    {
        if (this.card != card)
        {
            return;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        GameCardToCardBehavior.Add(card, this);
    }

    private void OnDestroy()
    {
        GameCardToCardBehavior.Remove(card);
    }
}

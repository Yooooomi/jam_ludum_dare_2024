using System.Collections.Generic;
using UnityEngine;

public class GamePlayer
{
    public readonly int id;
    public readonly GamePlayerStats stats;
    public readonly GamePlayerDeck deck;
    public readonly GameBoard board;
    public readonly List<GameCard> hand = new();

    public GamePlayer(int id, CardsCatalog catalog)
    {
        board = new GameBoard(id);
        deck = new GamePlayerDeck(id, board, catalog);
        this.id = id;
        stats = new GamePlayerStats();
        GameBridge.instance.onTurnBegin.AddListener(OnTurnBegin);
        GameBridge.instance.onTurnEnd.AddListener(OnTurnEnd);
    }

    public bool HasInHand(GameCard card)
    {
        return hand.Contains(card);
    }

    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i += 1)
        {
            hand.Add(deck.DrawCard());
        }
    }

    public void OnTurnEnd(int playerId)
    {
        if (id != playerId)
        {
            return;
        }
        stats.mana = stats.maxMana;
    }

    public void OnTurnBegin(int playerId)
    {
        if (id != playerId)
        {
            return;
        }
        DrawCards(2);
    }

    private bool CanConsumeMana(int amount)
    {
        return amount <= stats.mana;
    }

    public void RewardMana(int amount)
    {
        stats.mana = Mathf.Clamp(stats.mana + amount, 0, stats.maxMana);
        GameBridge.instance.onHeroStatChange.Invoke(this);
    }

    public bool ConsumeMana(int amount)
    {
        if (!CanConsumeMana(amount))
        {
            return false;
        }
        stats.mana -= amount;
        return true;
    }

    public bool CanPlayCard(GameCard card)
    {
        if (board.GetCardTile(card) == null)
        {
            return HasInHand(card) && CanConsumeMana(card.GetCardStats().mana);
        }
        else
        {
            return card.CanAttack();
        }
    }

    public bool PlayCard(GameBoardTile tile, GameCard card)
    {
        if (!CanPlayCard(card))
        {
            return false;
        }
        if (!board.CanPlaceCard(tile, card))
        {
            return false;
        }
        ConsumeMana(card.GetCardStats().mana);
        board.PlaceCard(tile, card);
        return true;
    }

    public bool GetAttacked(int damages)
    {
        stats.health -= damages;
        GameBridge.instance.onHeroStatChange.Invoke(this);
        return stats.health <= 0;
    }
}

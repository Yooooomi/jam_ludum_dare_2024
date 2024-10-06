using System.Collections.Generic;

public class GamePlayer
{
    public readonly int id;
    public readonly GamePlayerStats stats;
    public readonly GamePlayerDeck deck;
    public readonly GameBoard board = new();
    public readonly List<GameCard> hand = new();

    public GamePlayer(int id, CardsCatalog catalog)
    {
        deck = new GamePlayerDeck(id, catalog);
        this.id = id;
        stats = new GamePlayerStats();
        GameBridge.instance.onTurnBegin.AddListener(OnTurnBegin);
    }

    public bool Has(GameCard card)
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

    public void OnTurnBegin(int playerId)
    {
        if (id != playerId)
        {
            return;
        }
        DrawCards(2);
        stats.mana = stats.maxMana;
    }

    public bool ConsumeMana(int amount)
    {
        if (amount > stats.mana)
        {
            return false;
        }
        stats.mana -= amount;
        return true;
    }

    public bool PlayCard(GameBoardTile tile, GameCard card)
    {
        if (!Has(card))
        {
            return false;
        }
        if (!board.CanPlaceCard(tile, card))
        {
            return false;
        }
        if (!ConsumeMana(card.GetCardStats().mana)) {
            return false;
        }
        board.PlaceCard(tile, card);
        return true;
    }
}

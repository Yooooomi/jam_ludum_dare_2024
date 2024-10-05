public class GamePlayer
{
    public readonly int id;
    public readonly GamePlayerStats stats;
    public readonly GamePlayerDeck deck = new();
    public readonly GameBoard board;

    public GamePlayer(int id)
    {
        this.id = id;
        stats = new GamePlayerStats();
        GameBridge.instance.onTurnBegin.AddListener(OnTurnBegin);
    }

    public void OnTurnBegin(int playerId)
    {
        if (id != playerId)
        {
            return;
        }
        deck.DrawCards();
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
        if (!deck.Has(card))
        {
            return false;
        }
        if (stats.mana < card.GetCardStats().mana)
        {
            return false;
        }
        if (!board.CanPlaceCard(tile, card))
        {
            return false;
        }
        board.PlaceCard(tile, card);
        return true;
    }
}

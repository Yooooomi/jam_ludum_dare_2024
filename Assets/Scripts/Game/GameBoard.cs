using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameBoard
{
    private const int BOARD_WIDTH = 4;
    private const int BOARD_HEIGHT = 2;
    private readonly Dictionary<Vector2, GameBoardTile> tileByPos = new();
    private readonly Dictionary<GameBoardTile, Vector2> tileToPos = new();

    public GameBoard()
    {
        GameBridge.instance.onKilled.AddListener(OnKilled);
        for (int x = 0; x < BOARD_WIDTH; x += 1)
        {
            for (int y = 0; y < BOARD_HEIGHT; y += 1)
            {
                var pos = new Vector2(x, y);
                var tile = new GameBoardTile(x, y);
                tileByPos[pos] = tile;
                tileToPos[tile] = pos;
            }
        }
    }

    private GameCard GetCardNextToIt(GameCard card, Vector2 direction)
    {
        GameBoardTile tile = GetCardTile(card);
        if (tile == null)
        {
            return null;
        }
        Vector2 targetCardPos = tileToPos[tile] + direction;
        if (!tileByPos.TryGetValue(targetCardPos, out var cardNextToId))
        {
            return null;
        }
        if (cardNextToId.card == null)
        {
            return null;
        }
        return cardNextToId.card;
    }

    public List<GameCard> GetLine(GameCard card)
    {
        var cardTile = GetCardTile(card);
        if (cardTile == null)
        {
            return null;
        }
        int line = cardTile.y;
        var cards = new List<GameCard>();
        for (int i = 0; i < BOARD_WIDTH; i += 1)
        {
            var position = new Vector2(i, line);
            var tile = tileByPos[position];
            if (tile != null && tile.card != null)
            {
                cards.Add(tile.card);
            }
        }
        return cards;
    }

    public GameCard GetLeft(GameCard card)
    {
        return GetCardNextToIt(card, new Vector2(-1, 0));
    }

    public GameCard GetRight(GameCard card)
    {
        return GetCardNextToIt(card, new Vector2(1, 0));

    }

    public GameCard GetUp(GameCard card)
    {
        return GetCardNextToIt(card, new Vector2(0, 1));
    }

    public GameCard GetDown(GameCard card)
    {
        return GetCardNextToIt(card, new Vector2(0, -1));
    }

    public bool CanPlaceCard(GameBoardTile tile, GameCard card)
    {
        return tile.card == null;
    }

    public void PlaceCard(GameBoardTile tile, GameCard card)
    {
        if (tile == null)
        {
            Debug.LogError("PlaceCard with null tile");
            return;
        }
        if (!CanPlaceCard(tile, card))
        {
            Debug.LogError("Trying to PlaceCard on a tile that already have a card");
            return;
        }
        tile.card = card;
        GameBridge.instance.onPlaced.Invoke(card);
    }

    public GameBoardTile GetCardTile(GameCard card)
    {
        foreach (GameBoardTile tile in tileByPos.Values)
        {
            if (tile.card == card)
            {
                return tile;
            }
        }
        return null;
    }

    private void OnKilled(GameCard card)
    {
        CardRemoved(card);
    }

    private void CardRemoved(GameCard card)
    {
        var tile = GetCardTile(card);
        if (tile == null)
        {
            return;
        }
        tile.card = null;
    }

    public List<GameBoardTile> GetTiles()
    {
        return tileByPos.Values.ToList();
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameBoard
{
    private readonly Dictionary<Vector2, GameBoardTile> tileByPos = new();
    private readonly Dictionary<GameBoardTile, Vector2> tileToPos = new();

    public GameBoard()
    {
        GameBridge.instance.onKilled.AddListener(OnKilled);
        for (int x = 0; x < 4; x += 1)
        {
            for (int y = 0; y < 2; y += 1)
            {
                var pos = new Vector2(x, y);
                var tile = new GameBoardTile();
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
        GameBoardTile cardNextToId = tileByPos[targetCardPos];
        if (cardNextToId == null)
        {
            return null;
        }
        return cardNextToId.card;
    }

    public GameCard GetLeft(GameCard card)
    {
        return GetCardNextToIt(card, new Vector2(-1, 0));
    }
    public GameCard GetRight(GameCard card)
    {
        return GetCardNextToIt(card, new Vector2(-1, 0));

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

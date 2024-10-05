using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class OnTileClicked : UnityEvent<BoardTile> { };

public class PlayerBoard : MonoBehaviour
{
    [SerializeField]
    private BoardGenerator boardGenerator;

    public OnTileClicked onTileClicked = new OnTileClicked();

    private CardBehavior GetCardNextToIt(CardBehavior card, Vector2 direction)
    {
        BoardTile tile = GetCardTile(card);
        if (tile == null)
        {
            return null;
        }
        Vector2 targetCardPos = tileToPos[tile] + direction;
        BoardTile cardNextToId = tileByPos[targetCardPos];
        if (cardNextToId == null)
        {
            return null;
        }
        return cardNextToId.card;
    }

    public CardBehavior GetLeft(CardBehavior card)
    {
        return GetCardNextToIt(card, new Vector2(-1, 0));
    }
    public CardBehavior GetRight(CardBehavior card)
    {
        return GetCardNextToIt(card, new Vector2(-1, 0));

    }
    public CardBehavior GetUp(CardBehavior card)
    {
        return GetCardNextToIt(card, new Vector2(0, 1));
    }
    public CardBehavior GetDown(CardBehavior card)
    {
        return GetCardNextToIt(card, new Vector2(0, -1));
    }

    public bool CanPlaceCard(BoardTile tile, CardBehavior card)
    {
        return tile.card != null;
    }

    public void PlaceCard(BoardTile tile, CardBehavior card)
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
        card.transform.SetParent(tile.transform);
    }

    public BoardTile GetCardTile(CardBehavior card)
    {
        foreach (BoardTile tile in tileByPos.Values)
        {
            if (tile.card == card)
            {
                return tile;
            }
        }
        return null;
    }

    private void OnKilled(CardBehavior card)
    {
        CardRemoved(card);
    }

    private void CardRemoved(CardBehavior card)
    {
        BoardTile tile = GetCardTile(card);
        if (tile == null)
        {
            return;
        }
        tile.card = null;
    }

    // Internal implementation details below
    private readonly Dictionary<Vector2, BoardTile> tileByPos = new();
    private readonly Dictionary<BoardTile, Vector2> tileToPos = new();

    public List<BoardTile> GetTiles()
    {
        return tileByPos.Values.ToList();
    }

    private void Start()
    {
        foreach (Transform tile in boardGenerator.GetTiles())
        {
            Vector2 tile_pos = tile.GetComponent<TilePos>().pos;
            if (!tile.TryGetComponent<BoardTile>(out var tile_script))
            {
                Debug.LogError("Missing BoardTile script on tile");
                continue;
            }
            tileByPos[tile_pos] = tile_script;
            tileToPos[tile_script] = tile_pos;
            tile.GetComponent<Clickable>().OnClick.AddListener(() =>
            {
                onTileClicked.Invoke(tile_script);
            });
        }
    }


}

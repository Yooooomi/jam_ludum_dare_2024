using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class OnTileClicked : UnityEvent<BoardTile> { };

public class PlayerBoard : MonoBehaviour
{
    [SerializeField]
    private BoardGenerator boardGenerator;

    private readonly Dictionary<Vector2, BoardTile> tileByPos = new();
    private readonly Dictionary<BoardTile, Vector2> tileToPos = new();

    public OnTileClicked onTileClicked = new();

    private void Start()
    {
        var playerId = GetComponentInParent<PlayerInstance>().playerId;
        var gameTiles = GameState.instance.GetPlayer(playerId).board.GetTiles();
        var tiles = boardGenerator.GetTiles();
        for (int i = 0; i < gameTiles.Count; i += 1)
        {
            var tile = tiles[i];
            Vector2 tile_pos = tile.GetComponent<TilePos>().pos;
            if (!tile.TryGetComponent<BoardTile>(out var tile_script))
            {
                Debug.LogError("Missing BoardTile script on tile");
                continue;
            }
            tile_script.tile = gameTiles[i];
            tileByPos[tile_pos] = tile_script;
            tileToPos[tile_script] = tile_pos;
            tile.GetComponent<Clickable>().OnClick.AddListener(() =>
            {
                onTileClicked.Invoke(tile_script);
            });
        }
    }

    public void PlaceCardOnTile(CardBehavior card, GameBoardTile tile)
    {
        var boardTile = GetBoardTileFromGameTile(tile);
        if (boardTile == null)
        {
            return;
        }
        card.transform.SetParent(boardTile.transform);
        boardTile.card = card;
    }

    private BoardTile GetBoardTileFromGameTile(GameBoardTile tile)
    {
        foreach (var boardTile in tileByPos.Values)
        {
            if (boardTile.tile == tile)
            {
                return boardTile;
            }
        }
        return null;
    }

    public List<BoardTile> GetTiles()
    {
        return tileByPos.Values.ToList();
    }
}

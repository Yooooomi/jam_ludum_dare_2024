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

    public List<BoardTile> GetTiles()
    {
        return tileByPos.Values.ToList();
    }
}

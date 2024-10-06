using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    private static Dictionary<GamePlayer, PlayerInstance> gamePlayerToPlayerInstance = new();

    public static PlayerInstance FromGamePlayer(GamePlayer player)
    {
        gamePlayerToPlayerInstance.TryGetValue(player, out var result);
        return result;
    }

    public GamePlayer player => GameState.instance.players[id];
    [SerializeField]
    private int id;

    private void Awake() {
        gamePlayerToPlayerInstance.Add(player, this);
    }
}

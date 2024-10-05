using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RendererGameState : MonoBehaviour
{
    public static RendererGameState instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    protected List<GameObject> players = new();

    public GameObject GetPlayer(int playerId)
    {
        return players[playerId];
    }

    public GameObject GetOtherPlayer(int playerId)
    {
        return players[(playerId + 1) % 2];
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }
        GameState.instance.EndTurn();
    }
}

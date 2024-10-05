using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState instance;

    private void Awake()
    {
        instance = this;
    }

    protected int playerTurn = 1;
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

    private void Start()
    {
        EndTurn();
    }

    public bool MyTurn(int playerId) {
        return playerTurn == playerId;
    }

    public void EndTurn()
    {
        var player = players[playerTurn];
        player.SendMessage("OnTurnEnd", playerTurn);
        playerTurn = (playerTurn + 1) % 2;
        var nextPlayer = players[playerTurn];
        nextPlayer.SendMessage("OnTurnBegin", playerTurn);
    }
}

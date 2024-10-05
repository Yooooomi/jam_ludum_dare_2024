using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState instance;

    private void Awake() {
        instance = this;
    }

    protected int playerTurn;
    [SerializeField]
    protected List<GameObject> players = new();

    public void EndTurn() {
        var player = players[playerTurn];
        player.SendMessage("OnTurnEnd");
        playerTurn = (playerTurn + 1) % 2;
        var nextPlayer = players[playerTurn];
        nextPlayer.SendMessage("OnTurnBegin");
    }
}

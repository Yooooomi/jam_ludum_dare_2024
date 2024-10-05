using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
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

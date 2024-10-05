using System.Collections.Generic;

public class GameState
{
    public static GameState instance = new();
    public List<GamePlayer> players = new();

    private int playerTurn = 0;

    public GameState()
    {
        int id = 0;
        players.Add(new GamePlayer(id));
        id += 1;
        players.Add(new GamePlayer(id));
    }

    public void EndTurn()
    {
        GameBridge.instance.onTurnEnd.Invoke(playerTurn);
        playerTurn = (playerTurn + 1) % 2;
        GameBridge.instance.onTurnBegin.Invoke(playerTurn);
    }

    public bool MyTurn(int playerId)
    {
        return playerTurn == playerId;
    }

    public GamePlayer GetPlayer(int playerId)
    {
        return players[playerId];
    }

    public GamePlayer GetOtherPlayer(int playerId)
    {
        return players[(playerId + 1) % 2];
    }
}

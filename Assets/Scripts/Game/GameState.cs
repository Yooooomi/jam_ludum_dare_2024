using System.Collections.Generic;

public class GameState
{
    public static GameState instance;
    public static void InitGameState(CardsCatalog catalog)
    {
        GameBridge.Initialize();
        instance = new GameState(catalog);
    }
    public List<GamePlayer> players = new();

    private int playerTurn = 0;

    public GameState(CardsCatalog catalog)
    {
        players.Add(new GamePlayer(0, catalog));
        players.Add(new GamePlayer(1, catalog));
    }

    public void StartGame()
    {
        foreach (var player in players)
        {
            player.DrawCards(1);
        }
        GameBridge.instance.onTurnBegin.Invoke(playerTurn);
    }

    public void EndTurn()
    {
        playerTurn = (playerTurn + 1) % 2;
        GameBridge.instance.onTurnEnd.Invoke((playerTurn + 1) % 2);
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

using UnityEngine.Events;

public class GameOnPlaced : UnityEvent<GameCard> { }
public class GameOnKilled : UnityEvent<GameCard> { }
public class GameOnTurnBegin : UnityEvent<int> { }
public class GameOnTurnEnd : UnityEvent<int> { }
public class GameOnCardStatChange : UnityEvent<GameCard> { }
public class GameOnAttack : UnityEvent<GameCard, GameCard> { }
public class GameOnPlayerDrawCard : UnityEvent<GameCard> { }

public class GameBridge
{
    public GameOnPlaced onPlaced = new();
    public GameOnKilled onKilled = new();
    public GameOnTurnBegin onTurnBegin = new();
    public GameOnTurnEnd onTurnEnd = new();
    public GameOnCardStatChange onCardStatChange = new();
    public GameOnAttack onAttack = new();
    public GameOnPlayerDrawCard onPlayerDrawCard = new();

    public static GameBridge instance = new();
}

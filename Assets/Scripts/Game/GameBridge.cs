using UnityEngine.Events;

public class GameOnPlaced : UnityEvent<GameCard> { }
public class GameOnKilled : UnityEvent<GameCard> { }
public class GameOnTurnBegin : UnityEvent<int> { }
public class GameOnTurnEnd : UnityEvent<int> { }
public class GameOnCardStatChange : UnityEvent<GameCard, GameCardStats> { }
public class GameOnAttack : UnityEvent<GameCard, GameCard> { }
public class GameOnHeroAttack : UnityEvent<GameCard, GamePlayer> { }
public class GameOnHeroStatChange : UnityEvent<GamePlayer> { }
public class GameOnPlayerDrawCard : UnityEvent<GameCard> { }

public class GameBridge
{
    public GameOnPlaced onPlaced = new();
    public GameOnKilled onKilled = new();
    public GameOnTurnBegin onTurnBegin = new();
    public GameOnTurnEnd onTurnEnd = new();
    public GameOnCardStatChange onCardStatChange = new();
    public GameOnAttack onAttack = new();
    public GameOnHeroAttack onHeroAttack = new();
    public GameOnHeroStatChange onHeroStatChange = new();
    public GameOnPlayerDrawCard onPlayerDrawCard = new();

    public static GameBridge instance;

    public static void Initialize()
    {
        instance = new();
    }
}

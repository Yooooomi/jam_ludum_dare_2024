using UnityEngine;
using UnityEngine.Events;

public class GameOnPlaced : UnityEvent<GameCard> { }
public class GameOnKilled : UnityEvent<GameCard> { }
public class GameOnTurnBegin : UnityEvent<int> { }
public class GameOnTurnEnd : UnityEvent<int> { }
// victim
public class GameOnDamageTaken : UnityEvent<GameCard> { }
// from
public class GameOnAttack : UnityEvent<GameCard> { }
public class GameOnCardUpdate : UnityEvent<GameCard> { }
public class GameOnPlayerDrawCard : UnityEvent<GameCard> { }

public class GameBridge : MonoBehaviour
{
    public GameOnPlaced onPlaced = new();
    public GameOnKilled onKilled = new();
    public GameOnTurnBegin onTurnBegin = new();
    public GameOnTurnEnd onTurnEnd = new();
    public GameOnDamageTaken onDamageTaken = new();
    public GameOnAttack onAttack = new();
    public GameOnCardUpdate onCardUpdate = new();
    public GameOnPlayerDrawCard onPlayerDrawCard = new();

    public static GameBridge instance = new();
}

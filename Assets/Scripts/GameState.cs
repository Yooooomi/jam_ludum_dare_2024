using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnPlaced : UnityEvent<CardBehavior> { }
public class OnKilled : UnityEvent<CardBehavior> { }
public class OnTurnBegin : UnityEvent<int> { }
public class OnTurnEnd : UnityEvent<int> { }
// victim
public class OnDamageTaken : UnityEvent<CardBehavior> { }
// from
public class OnAttack : UnityEvent<CardBehavior> { }

public class GameState : MonoBehaviour
{
    public static GameState instance;

    public OnPlaced onPlaced = new();
    public OnKilled onKilled = new();
    public OnTurnBegin onTurnBegin = new();
    public OnTurnEnd onTurnEnd = new();
    public OnDamageTaken onDamageTaken = new();
    public OnAttack onAttack = new();

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

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }
        EndTurn();
    }

    public bool MyTurn(int playerId)
    {
        return playerTurn == playerId;
    }

    public void EndTurn()
    {
        onTurnEnd.Invoke(playerTurn);
        playerTurn = (playerTurn + 1) % 2;
        onTurnBegin.Invoke(playerTurn);
    }
}

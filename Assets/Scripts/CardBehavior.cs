using UnityEngine;

public abstract class CardBehavior : MonoBehaviour
{
    public Card card;

    protected virtual void OnSelfPlaced()
    {
        Debug.Log("OnSelfPlaced");
    }

    protected virtual void OnPlaced()
    {
        Debug.Log("OnPlaced");
    }

    protected virtual void OnKilled()
    {
        Debug.Log("OnKilled");
    }

    protected virtual void OnTurnBegin()
    {
        Debug.Log("OnTurnBegin");
    }

    protected virtual void OnTurnEnd()
    {
        Debug.Log("OnTurnEnd");
    }

    protected virtual void OnDamageTaken(Card from)
    {
        Debug.Log("OnDamageTaken");
    }

    protected virtual void OnAttack(Card target)
    {
        Debug.Log("OnAttack");
    }

}

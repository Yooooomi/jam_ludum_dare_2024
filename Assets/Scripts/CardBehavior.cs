using UnityEngine;
using UnityEngine.Events;

public abstract class CardBehavior : MonoBehaviour
{
    public GameCard card;

    public UnityEvent onStatChanged = new();

    public bool IsDamageBuffed()
    {
        return false;
    }

    public bool IsHealthBuffed()
    {
        return false;
    }
}

using UnityEngine;

public abstract class PlayerBoard : MonoBehaviour
{
    public abstract CardBehavior GetLeft(CardBehavior self);
    public abstract CardBehavior GetRight(CardBehavior self);
    public abstract CardBehavior GetUp(CardBehavior self);
    public abstract CardBehavior GetDown(CardBehavior self);
    public abstract void PlaceCard(CardBehavior card);
}

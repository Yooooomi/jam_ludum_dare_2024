using UnityEngine;

public class CardManipulation : MonoBehaviour
{
    private Deck deck;
    private CardBehavior behavior;

    private void Start()
    {
        deck = GetComponentInParent<Deck>();
        behavior = GetComponent<CardBehavior>();
    }

    public void Click()
    {
        deck.Select(behavior);
    }
}

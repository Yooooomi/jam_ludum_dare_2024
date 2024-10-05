using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDeckUpdate : UnityEvent { }

public class Deck : MonoBehaviour
{
    public CardBehavior selected;
    [SerializeField]
    public Transform deckHand;
    public List<CardBehavior> deck = new();
    [HideInInspector]
    public List<CardBehavior> hand = new();

    public OnDeckUpdate onDeckUpdate = new();

    private void OnTurnEnd()
    {
        selected = null;
    }

    public void Select(CardBehavior card)
    {
        selected = card;
        onDeckUpdate.Invoke();
        // TODO detect if card is ennemy card and attack
        // hand.Remove(selected);
    }

    public void DrawCard()
    {
        var card = deck[0];
        var instantiated = Instantiate(card, deckHand);
        hand.Add(instantiated);
        onDeckUpdate.Invoke();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawCard();
        }
    }
}

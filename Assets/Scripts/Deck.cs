using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public CardBehavior selected;
    public List<CardBehavior> deck = new();
    [HideInInspector]
    public List<CardBehavior> hand = new();

    private void OnTurnEnd()
    {
        selected = null;
    }

    public void Select(CardBehavior card)
    {
        if (selected == null)
        {
            selected = card;
            return;
        }
        // TODO detect if card is ennemy card and attack
        // hand.Remove(selected);
    }

    public void DrawCard()
    {
        var card = deck[0];
        var instantiated = Instantiate(card, transform);
        hand.Add(instantiated);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawCard();
        }
    }
}

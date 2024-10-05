using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public int startCardsCount;
    public int cardsPerTurn;

    private Deck deck;

    private void Start()
    {
        deck = GetComponent<Deck>();
        deck.DrawCards(startCardsCount);
    }

    private void OnTurnBegin(int playerId)
    {
        if (id != playerId)
        {
            return;
        }
        deck.DrawCards(cardsPerTurn);
    }
}

using UnityEngine;

public class DeckRenderer : MonoBehaviour
{
    [SerializeField]
    private float cardOffset;
    [SerializeField]
    private float selectedOffset;
    [SerializeField]
    private float angle;
    private Deck deck;

    private void Start()
    {
        deck = GetComponent<Deck>();
        deck.onDeckUpdate.AddListener(UpdateDeck);
    }

    private void UpdateDeck()
    {
        var center = deck.hand.Count * cardOffset / 2;
        var deckHandPosition = deck.deckHand.position;
        for (int i = 0; i < deck.hand.Count; i += 1)
        {
            var card = deck.hand[i];
            var selected = deck.selectedCard.card == card;
            var height = selected ? selectedOffset : 0;
            var finalAngle = selected ? 0 : angle;
            var cardAnimation = card.GetComponent<CardPositionAnimation>();

            var position = new Vector3(
                deckHandPosition.x + i * cardOffset - center,
                0,
                deckHandPosition.z + height
            );
            var rotation = Quaternion.Euler(0, 0, finalAngle);

            cardAnimation.GoTo(position, rotation);
        }
    }
}

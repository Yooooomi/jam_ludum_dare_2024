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
    }

    private void Update()
    {
        var center = deck.hand.Count * cardOffset / 2;
        for (int i = 0; i < deck.hand.Count; i += 1)
        {
            var card = deck.hand[i];
            var selected = deck.selected == card;
            var height = selected ? selectedOffset : 0;
            var finalAngle = selected ? 0 : angle;
            card.transform.SetLocalPositionAndRotation(new Vector3(i * cardOffset - center, 0, height), Quaternion.Euler(0, 0, finalAngle));
        }
    }
}

// player board
// player board tile -> on click, player->deck->OnTileClick(index)
// player board tile -> PlaceCard(CardBehavior);

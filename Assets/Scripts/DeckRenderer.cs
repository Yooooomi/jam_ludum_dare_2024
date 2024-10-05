using UnityEngine;

public class DeckRenderer : MonoBehaviour
{
    [SerializeField]
    private float handYOffset;
    [SerializeField]
    private float cardOffset;
    [SerializeField]
    private float selectedOffset;
    [SerializeField]
    private float angle;
    private Deck deck;
    private PlayerBoard ourBoard;

    private void Awake()
    {
        deck = GetComponent<Deck>();
        deck.onSelectionUpdate.AddListener(UpdateDeck);
        DelayedGameBridge.instance.onPlayerDrawCard.AddListener(OnPlaced);
        DelayedGameBridge.instance.onPlaced.AddListener(OnPlaced);
        ourBoard = GetComponent<PlayerBoard>();
    }

    private void OnPlaced(GameCard card)
    {
        if (deck.playerId != card.playerId) {
            return;
        }
        var tile = GameState.instance.GetPlayer(card.playerId).board.GetCardTile(card);
        if (tile != null) {
            var cardBehavior = CardBehavior.FromGameCard(card);
            if (cardBehavior != null) {
                ourBoard.PlaceCardOnTile(cardBehavior, tile);
            }
        }
        UpdateDeck();
    }

    private void UpdateDeck()
    {
        var center = deck.hand.Count * cardOffset / 2;
        var deckHandPosition = deck.deckHand.position;

        for (int i = 0; i < deck.hand.Count; i += 1)
        {
            var card = deck.hand[i];
            var selected = deck.selectedCard != null && deck.selectedCard.card == card;
            var height = selected ? selectedOffset : 0;
            var finalAngle = selected ? 0 : angle;
            var cardAnimation = card.GetComponent<CardPositionAnimation>();

            var position = new Vector3(
                deckHandPosition.x + i * cardOffset - center,
                handYOffset,
                deckHandPosition.z + height
            );
            var rotation = Quaternion.Euler(0, 0, finalAngle);

            cardAnimation.GoTo(position, rotation);
        }

        var tiles = ourBoard.GetTiles();
        for (int i = 0; i < tiles.Count; i += 1)
        {
            var tile = tiles[i];
            var card = tile.card;
            if (card == null)
            {
                continue;
            }
            var offsetWithTile = new Vector3(0, 0.1f, 0);
            card.GetComponent<CardPositionAnimation>().GoTo(tile.transform.position + offsetWithTile, Quaternion.identity);
        }
    }
}

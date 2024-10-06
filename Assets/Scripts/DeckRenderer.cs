using System.Collections;
using UnityEngine;

public class DeckRenderer : MonoBehaviour
{
    [SerializeField]
    private float boardYOffset;
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
        DelayedGameBridge.instance.onAttack.AddListener(OnAttack);
        ourBoard = GetComponent<PlayerBoard>();
    }

    private IEnumerator PlayAttackAnimation(CardPositionAnimation aggressor, CardPositionAnimation victim)
    {
        aggressor.GoTo(victim.transform.position, Quaternion.identity, 0.15f);
        yield return new WaitForSeconds(0.15f);
        UpdateDeck();
    }

    private void OnAttack(GameCard aggressor, GameCard victim)
    {
        var aggressorCard = CardBehavior.FromGameCard(aggressor);
        var victimCard = CardBehavior.FromGameCard(victim);

        if (aggressorCard == null || victimCard == null)
        {
            return;
        }
        var aggressorAnimation = aggressorCard.GetComponent<CardPositionAnimation>();
        var victimAnimation = victimCard.GetComponent<CardPositionAnimation>();
        if (aggressorAnimation == null || victimAnimation == null)
        {
            return;
        }
        StartCoroutine(PlayAttackAnimation(aggressorAnimation, victimAnimation));
    }

    private void OnPlaced(GameCard card)
    {
        if (deck.playerId != card.playerId)
        {
            return;
        }
        var tile = GameState.instance.GetPlayer(card.playerId).board.GetCardTile(card);
        if (tile != null)
        {
            var cardBehavior = CardBehavior.FromGameCard(card);
            if (cardBehavior != null)
            {
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
            if (deck.selectedCard != null && deck.selectedCard.card == card)
            {
                card.GetComponent<CardPositionAnimation>().GoTo(tile.transform.position + offsetWithTile + Vector3.up * boardYOffset, Quaternion.identity);
            }
            else
            {
                card.GetComponent<CardPositionAnimation>().GoTo(tile.transform.position + offsetWithTile, Quaternion.identity);
            }
        }
    }
}

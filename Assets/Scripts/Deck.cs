using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnSelectionUpdate : UnityEvent { }

public class SelectedCard
{
    public enum Location
    {
        OUR = 0,
        THEIR = 1,
        HAND = 2,
    }

    public Location location;
    public CardBehavior card;

    public SelectedCard(Location location, CardBehavior card)
    {
        this.location = location;
        this.card = card;
    }
}

public class Deck : MonoBehaviour
{
    [SerializeField]
    public Transform deckHand;
    public List<CardBehavior> deck = new();
    [HideInInspector]
    public List<CardBehavior> hand = new();

    public OnSelectionUpdate onSelectionUpdate = new();

    private PlayerBoard ourBoard;
    private PlayerBoard theirBoard;

    public SelectedCard selectedCard;
    [HideInInspector]
    public int playerId;

    private void Awake()
    {
        playerId = GetComponent<PlayerInstance>().player.id;
        DelayedGameBridge.instance.onTurnEnd.AddListener(OnTurnEnd);
        DelayedGameBridge.instance.onPlayerDrawCard.AddListener(OnPlayerDrawCard);
        DelayedGameBridge.instance.onPlaced.AddListener(OnPlaced);
    }

    private void Start()
    {
        var us = RendererGameState.instance.GetPlayer(playerId);
        var them = RendererGameState.instance.GetOtherPlayer(playerId);

        ourBoard = us.GetComponent<PlayerBoard>();
        theirBoard = them.GetComponent<PlayerBoard>();
        ourBoard.onTileClicked.AddListener(OnOurBoardClick);
        theirBoard.onTileClicked.AddListener(OnTheirBoardClick);

        us.GetComponentInChildren<Hero>().onClick.AddListener(OnOurHeroClicked);
        them.GetComponentInChildren<Hero>().onClick.AddListener(OnTheirHeroClicked);
    }

    private void OnOurHeroClicked()
    {
        ChangeCardSelection(null);
    }

    private void OnTheirHeroClicked()
    {
        if (selectedCard == null || selectedCard.location != SelectedCard.Location.OUR)
        {
            ChangeCardSelection(null);
            return;
        }
        selectedCard.card.card.AttackHero(GameState.instance.GetOtherPlayer(playerId));
        selectedCard = null;
    }

    private void OnPlaced(GameCard card)
    {
        if (playerId != card.playerId)
        {
            return;
        }
        var cardBehavior = CardBehavior.FromGameCard(card);
        hand.Remove(cardBehavior);
    }

    private void OnPlayerDrawCard(GameCard card)
    {
        if (card.playerId != playerId)
        {
            return;
        }
        var cardGameobject = CardsCatalog.instance.InstantiateCard(card.info);
        var setup = cardGameobject.GetComponent<CardInitialSetup>();
        setup.SetupGameCard(card);
        hand.Add(setup.card);
        onSelectionUpdate.Invoke();
    }

    private void OnTurnEnd(int playerId)
    {
        ChangeCardSelection(null);
    }

    private void OnOurBoardClick(BoardTile tile)
    {
        if (!GameState.instance.MyTurn(playerId))
        {
            return;
        }
        if (selectedCard == null)
        {
            return;
        }
        if (selectedCard.location == SelectedCard.Location.HAND)
        {
            GameState.instance.GetPlayer(playerId).PlayCard(tile.tile, selectedCard.card.card);
        }
        ChangeCardSelection(null);
    }

    private void OnTheirBoardClick(BoardTile tile)
    {
        if (!GameState.instance.MyTurn(playerId))
        {
            return;
        }
        ChangeCardSelection(null);
    }

    private void ChangeCardSelection(SelectedCard selectedCard)
    {
        this.selectedCard = selectedCard;
        onSelectionUpdate.Invoke();
    }

    public void Select(CardBehavior card)
    {
        if (!GameState.instance.MyTurn(playerId))
        {
            return;
        }
        var player = GameState.instance.GetPlayer(playerId);
        var ourBoardTile = player.board.GetCardTile(card.card);
        if (ourBoardTile != null)
        {
            var isNewCardOrNotAlreadyChosen = selectedCard == null || selectedCard.card != card;
            if (isNewCardOrNotAlreadyChosen && player.CanPlayCard(card.card))
            {
                ChangeCardSelection(new SelectedCard(SelectedCard.Location.OUR, card));
                return;
            }
            ChangeCardSelection(null);
            return;
        }

        var theirBoardTile = GameState.instance.GetOtherPlayer(playerId).board.GetCardTile(card.card);
        if (
            // Their board has a tile containing the card
            theirBoardTile != null && theirBoardTile.HoldsCard() &&
            // Our selected card comes from our board
            selectedCard != null && selectedCard.location == SelectedCard.Location.OUR
        )
        {
            if (!selectedCard.card.card.CanAttack())
            {
                ChangeCardSelection(null);
                return;
            }
            selectedCard.card.card.Attack(theirBoardTile.card);
            // Assigning instead of calling function to avoid triggering an animation
            selectedCard = null;
            return;
        }

        if (selectedCard != null && selectedCard.card == card)
        {
            ChangeCardSelection(null);
        }
        else if (hand.Contains(card))
        {
            ChangeCardSelection(new SelectedCard(SelectedCard.Location.HAND, card));
        }
    }
}

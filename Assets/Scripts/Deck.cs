using System;
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
    public int playerId;
    [SerializeField]
    public Transform deckHand;
    public List<CardBehavior> deck = new();
    [HideInInspector]
    public List<CardBehavior> hand = new();

    public OnSelectionUpdate onSelectionUpdate = new();

    private PlayerStats playerStats;
    private PlayerBoard ourBoard;
    private PlayerBoard theirBoard;

    public SelectedCard selectedCard;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        ourBoard = GameState.instance.GetPlayer(playerId).GetComponent<PlayerBoard>();
        theirBoard = GameState.instance.GetOtherPlayer(playerId).GetComponent<PlayerBoard>();

        ourBoard.onTileClicked.AddListener(OnOurBoardClick);
        theirBoard.onTileClicked.AddListener(OnTheirBoardClick);
    }

    private void OnTurnEnd()
    {
        ChangeCardSelection(null);
    }

    private void OnOurBoardClick(BoardTile tile)
    {
        if (tile.card != null || selectedCard == null)
        {
            return;
        }
        if (selectedCard.location == SelectedCard.Location.HAND)
        {
            if (!hand.Contains(selectedCard.card) || !ourBoard.CanPlaceCard(tile, selectedCard.card))
            {
                ChangeCardSelection(null);
                return;
            }
            if (!playerStats.ConsumeMana(selectedCard.card.GetCardStats().mana))
            {
                return;
            }
            ourBoard.PlaceCard(tile, selectedCard.card);
            hand.Remove(selectedCard.card);
            ChangeCardSelection(null);
        }
    }

    private void OnTheirBoardClick(BoardTile tile)
    {
        Debug.Log("Their board click");
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
        var ourBoardTile = ourBoard.GetCardTile(card);
        if (ourBoardTile != null)
        {
            ChangeCardSelection(new SelectedCard(SelectedCard.Location.OUR, card));
            return;
        }

        var theirBoardTile = theirBoard.GetCardTile(card);
        if (
            // Their board has a tile containing the card
            theirBoardTile != null && theirBoardTile.card != null &&
            // Our selected card comes from our board
            selectedCard != null && selectedCard.location == SelectedCard.Location.OUR
        )
        {
            if (!selectedCard.card.CanAttack())
            {
                return;
            }
            selectedCard.card.Attack(theirBoardTile.card);
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

    public void DrawCards(int count)
    {
        for (int i = 0; i < count; i += 1)
        {
            var card = deck[0];
            var instantiated = Instantiate(card, deckHand);
            instantiated.playerId = playerId;
            hand.Add(instantiated);
            onSelectionUpdate.Invoke();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawCards(1);
        }
    }
}

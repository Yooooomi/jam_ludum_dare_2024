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

    private PlayerBoard ourBoard;
    private PlayerBoard theirBoard;

    public SelectedCard selectedCard;

    private void Start()
    {
        DelayedGameBridge.instance.onTurnEnd.AddListener(OnTurnEnd);

        ourBoard = RendererGameState.instance.GetPlayer(playerId).GetComponent<PlayerBoard>();
        theirBoard = RendererGameState.instance.GetOtherPlayer(playerId).GetComponent<PlayerBoard>();

        ourBoard.onTileClicked.AddListener(OnOurBoardClick);
        theirBoard.onTileClicked.AddListener(OnTheirBoardClick);
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
            ChangeCardSelection(null);
        }
    }

    private void OnTheirBoardClick(BoardTile tile)
    {
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
        var ourBoardTile = GameState.instance.GetPlayer(playerId).board.GetCardTile(card.card);
        if (ourBoardTile != null)
        {
            ChangeCardSelection(new SelectedCard(SelectedCard.Location.OUR, card));
            return;
        }

        var theirBoardTile = GameState.instance.GetOtherPlayer(playerId).board.GetCardTile(card.card);
        Debug.Log($"{playerId} Their {theirBoardTile} {(theirBoardTile != null ? theirBoardTile.HoldsCard() : "none")} {selectedCard} {(selectedCard != null ? selectedCard.location : "none")}");
        if (
            // Their board has a tile containing the card
            theirBoardTile != null && theirBoardTile.HoldsCard() &&
            // Our selected card comes from our board
            selectedCard != null && selectedCard.location == SelectedCard.Location.OUR
        )
        {
            if (!selectedCard.card.card.CanAttack())
            {
                return;
            }
            selectedCard.card.card.Attack(theirBoardTile.card);
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

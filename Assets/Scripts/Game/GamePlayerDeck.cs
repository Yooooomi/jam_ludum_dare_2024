using System.Collections.Generic;
using UnityEngine;

public class GamePlayerDeck
{
  private int playerId;
  public List<GameCard> cards = new();
  private CardsCatalog catalog;

  public GamePlayerDeck(int playerId, CardsCatalog catalog)
  {
    this.playerId = playerId;
    this.catalog = catalog;
    ResetDeck();
  }

  public GameCard DrawCard()
  {
    if (cards_.Count == 0)
    {
      ResetDeck();
    }
    int index = Random.Range(0, cards_.Count);
    var pickedCard = cards_[index];
    cards_.RemoveAt(index);
    var card = new GameCard(pickedCard.stats, pickedCard)
    {
      playerId = playerId,
    };
    GameBridge.instance.onPlayerDrawCard.Invoke(card);
    return card;
  }

  private void ResetDeck()
  {
    cards_ = new List<CardInfo>();
    foreach (CardInfo card in catalog.cardInfos)
    {
      for (int i = 0; i < card.count; i++)
      {
        cards_.Add(card);
      }
    }
  }


  private List<CardInfo> cards_;
}

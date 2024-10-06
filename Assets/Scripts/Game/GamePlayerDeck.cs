using System.Collections.Generic;
using UnityEngine;

public class GamePlayerDeck
{
  private readonly int playerId;
  public List<GameCard> cards = new();
  private readonly CardsCatalog catalog;
  private readonly GameBoard board;

  public GamePlayerDeck(int playerId, GameBoard board, CardsCatalog catalog)
  {
    this.playerId = playerId;
    this.catalog = catalog;
    this.board = board;
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
    var card = GetGameCardFromBehavior(pickedCard.stats, pickedCard);
    card.Setup(playerId, board, pickedCard.stats, pickedCard);
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

  private static GameCard GetGameCardFromBehavior(GameCardStats stats, CardInfo info)
  {
    switch (info.cardBehaviorType)
    {
      case CardBehaviorType.TinyCreature:
        return new TinyCreature();
      case CardBehaviorType.MightGuardian:
        return new MightGuardian();
      case CardBehaviorType.ValorWarden:
        return new ValorWarden();
      case CardBehaviorType.IronbarkProtector:
        return new IronbarkProtector();
      case CardBehaviorType.LifetenderSentinel:
        return new LifetenderSentinel();
      case CardBehaviorType.HeartwoodShielder:
        return new HeartwoodShielder();
      case CardBehaviorType.BladeleafWatcher:
        return new BladeleafWatcher();
      case CardBehaviorType.SoulmenderKeeper:
        return new SoulmenderKeeper();
      case CardBehaviorType.Shiftwarden:
        return new Shiftwarden();
      case CardBehaviorType.LifeweaverGuardian:
        return new LifeweaverGuardian();
      case CardBehaviorType.VerdantSummoner:
        return new VerdantSummoner();
      case CardBehaviorType.BulwarkProtector:
        return new BulwarkProtector();
    }
    return new GameCard();
  }
}

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
    return info.cardBehaviorType switch
    {
      CardBehaviorType.TinyCreature => new TinyCreature(),
      CardBehaviorType.MightGuardian => new MightGuardian(),
      CardBehaviorType.ValorWarden => new ValorWarden(),
      CardBehaviorType.IronbarkProtector => new IronbarkProtector(),
      CardBehaviorType.LifetenderSentinel => new LifetenderSentinel(),
      CardBehaviorType.HeartwoodShielder => new HeartwoodShielder(),
      CardBehaviorType.BladeleafWatcher => new BladeleafWatcher(),
      CardBehaviorType.SoulmenderKeeper => new SoulmenderKeeper(),
      CardBehaviorType.LifeweaverGuardian => new LifeweaverGuardian(),
      CardBehaviorType.VerdantSummoner => new VerdantSummoner(),
      CardBehaviorType.BulwarkProtector => new BulwarkProtector(),
      CardBehaviorType.BloodbondBoon => new BloodbondBoon(),
      CardBehaviorType.DeathsInsight => new DeathsInsight(),
      CardBehaviorType.HasteOfTheWilds => new HasteOfTheWilds(),
      CardBehaviorType.DoombringersCountdown => new DoombringersCountdown(),
      _ => new GameCard(),
    };
  }
}

using System.Collections.Generic;

public class GamePlayerDeck
{
  public List<GameCard> cards = new();

  public void DrawCards()
  {
    // TODO FIX
    var card = new GameCard();
    GameBridge.instance.onPlayerDrawCard.Invoke(card);
  }

  public bool Has(GameCard card)
  {
    return cards.Contains(card);
  }
}

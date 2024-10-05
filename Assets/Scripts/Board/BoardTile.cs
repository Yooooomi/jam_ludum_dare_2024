using UnityEngine;

public class BoardTile : MonoBehaviour
{
  public GameBoardTile tile;
  public CardBehavior card;

  private void Start()
  {
    DelayedGameBridge.instance.onKilled.AddListener(OnKilled);
  }

  private void OnKilled(GameCard card)
  {
    if (this.card == null || this.card.card != card)
    {
      return;
    }
    this.card = null;
  }
}

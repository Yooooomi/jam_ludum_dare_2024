using UnityEngine;

public class BoardTile : MonoBehaviour
{
  public GameBoardTile tile;
  public CardBehavior card;

  public bool HoldsCard()
  {
    return tile.card != null;
  }
}

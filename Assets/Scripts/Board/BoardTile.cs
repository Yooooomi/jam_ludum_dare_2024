using UnityEngine;

public class BoardTile : MonoBehaviour {
  public CardBehavior card;

  public bool HoldsCard() {
    return card != null;
  }
}

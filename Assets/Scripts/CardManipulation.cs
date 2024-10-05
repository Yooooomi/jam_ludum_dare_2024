using UnityEngine;

public class CardManipulation : MonoBehaviour
{
    private CardBehavior behavior;

    private void Start()
    {
        behavior = GetComponent<CardBehavior>();
    }

    public void Click()
    {
        var us = GameState.instance.GetPlayer(behavior.playerId);
        var them = GameState.instance.GetOtherPlayer(behavior.playerId);

        us.GetComponent<Deck>().Select(behavior);
        them.GetComponent<Deck>().Select(behavior);
    }
}

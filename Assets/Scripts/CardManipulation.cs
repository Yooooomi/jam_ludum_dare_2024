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
        var us = RendererGameState.instance.GetPlayer(behavior.card.playerId);
        var them = RendererGameState.instance.GetOtherPlayer(behavior.card.playerId);

        us.GetComponent<Deck>().Select(behavior);
        them.GetComponent<Deck>().Select(behavior);
    }
}

using UnityEngine;

public class CardActionabilityDisplay : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshToChangeColor;
    [SerializeField]
    private Material activeMaterial;
    [SerializeField]
    private Material passiveMaterial;

    private CardBehavior card;

    private void Start()
    {
        card = GetComponent<CardBehavior>();
        DelayedGameBridge.instance.onAll.AddListener(ComputePlayability);
    }

    private void ComputePlayability()
    {
        if (GameState.instance.MyTurn(card.card.playerId) && GameState.instance.GetPlayer(card.card.playerId).CanPlayCard(card.card))
        {
            meshToChangeColor.sharedMaterial = activeMaterial;
        }
        else
        {
            meshToChangeColor.sharedMaterial = passiveMaterial;
        }
    }
}

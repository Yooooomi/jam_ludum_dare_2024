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

    private void Awake()
    {
        DelayedGameBridge.instance.onTurnBegin.AddListener(OnTurnBegin);
        DelayedGameBridge.instance.onPlaced.AddListener(OnPlaced);
        DelayedGameBridge.instance.onAttack.AddListener(OnAttack);
        DelayedGameBridge.instance.onCardStatChange.AddListener(OnCardStatChange);
        DelayedGameBridge.instance.onTurnEnd.AddListener(OnTurnBegin);
    }

    private void Start()
    {
        card = GetComponent<CardBehavior>();
    }

    private void OnPlaced(GameCard card)
    {
        ComputePlayability();
    }

    private void OnTurnBegin(int playerId)
    {
        ComputePlayability();
    }

    private void OnAttack(GameCard from, GameCard to)
    {
        ComputePlayability();
    }

    private void OnCardStatChange(GameCard card, GameCardStats diff)
    {
        ComputePlayability();
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

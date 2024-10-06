using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        card = GetComponent<CardBehavior>();
    }

    void Update()
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

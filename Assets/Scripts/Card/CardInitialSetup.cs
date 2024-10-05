using UnityEngine;

public class CardInitialSetup : MonoBehaviour
{
    public MeshRenderer imageRender;

    public CardBehavior card;

    private void Awake()
    {
        card = GetComponent<CardBehavior>();
    }

    public void Setup(CardInfo cardInfo)
    {
        imageRender.sharedMaterial = new Material(imageRender.sharedMaterial)
        {
            mainTexture = cardInfo.image
        };
    }

    public void SetupGameCard(GameCard card)
    {
        this.card.card = card;
    }
}

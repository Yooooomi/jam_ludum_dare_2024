using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInitialSetup : MonoBehaviour
{
    public MeshRenderer imageRender;
    public CardBehavior cardBehavior;

    public void Setup(CardInfo cardInfo) {
        imageRender.sharedMaterial = new Material(imageRender.sharedMaterial)
        {
            mainTexture = cardInfo.image
        };
    }
}

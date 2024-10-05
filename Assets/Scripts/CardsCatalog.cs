using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public enum CardType
{
    Spectator = 0,
    TinyCreature = 1
};

[Serializable]
public enum CardBehaviorType
{
    TinyCreature,
    MightGuardian,
    ValorWarden,
    IronbarkProtector,
    LifetenderSentinel,
    HeartwoodShielder,
    BladeleafWatcher,
    SoulmenderKeeper,
    Shiftwarden,
    LifeweaverGuardian,
    VerdantSummoner,
    BulwarkProtector,
}

[Serializable]
public class CardInfo
{
    public Texture2D image;
    public string name;
    public CardStats stats;
    public int count;
    public string description;
    public CardType cardType;
    public CardBehaviorType cardBehaviorType;
}

public class PlayerDeck
{
    public PlayerDeck(CardsCatalog catalog)
    {
        catalog_ = catalog;
        ResetDeck();
    }

    public CardInfo PickCard()
    {
        if (cards_.Count == 0)
        {
            ResetDeck();
        }
        int index = UnityEngine.Random.Range(0, cards_.Count);
        CardInfo pickedCard = cards_[index];
        cards_.RemoveAt(index);
        return pickedCard;
    }

    private void ResetDeck()
    {
        cards_ = new List<CardInfo>();
        foreach (CardInfo card in catalog_.cardInfos)
        {
            for (int i = 0; i < card.count; i++)
            {
                cards_.Add(card);
            }
        }
    }


    private List<CardInfo> cards_;
    private CardsCatalog catalog_;
}

public class CardsCatalog : MonoBehaviour
{
    public List<CardInfo> cardInfos;

    public GameObject cardPrefab;
    public Transform cardsGenerationParent;

    public GameObject InstantiateCard(CardInfo cardInfo)
    {
        GameObject card = Instantiate(cardPrefab, cardsGenerationParent.position, Quaternion.identity, cardsGenerationParent);
        card.GetComponent<CardInitialSetup>().Setup(cardInfo);
        return card;
    }

    [CustomEditor(typeof(CardsCatalog))]
    public class ButtonInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("GenerateAllCards"))
            {
                var instance = (CardsCatalog)target;
                instance.GenerateAllCards();
            }
        }
    }

    private void GenerateAllCards()
    {
        for (int i = 0; i < cardInfos.Count; ++i)
        {
            CardInfo card = cardInfos[i];
            GameObject go = InstantiateCard(card);
            go.transform.position += new Vector3(i * 2.5f, 0, 0);
        }
    }
}

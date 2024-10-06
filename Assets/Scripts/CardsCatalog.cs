using System;
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
    LifeweaverGuardian,
    VerdantSummoner,
    BulwarkProtector,
    BloodbondBoon,
    DeathsInsight,
    HasteOfTheWilds,
    DoombringersCountdown,
}

[Serializable]
public class CardInfo
{
    public Texture2D image;
    public string name;
    public GameCardStats stats;
    public int count;
    public string description;
    public CardType cardType;
    public CardBehaviorType cardBehaviorType;
}

public class CardsCatalog : MonoBehaviour
{
    static public CardsCatalog instance;

    public List<CardInfo> cardInfos;

    public GameObject cardPrefab;
    public Transform cardsGenerationParent;

    private void Awake()
    {
        instance = this;
    }

    public GameObject InstantiateCard(CardInfo cardInfo)
    {
        GameObject card = Instantiate(cardPrefab, cardsGenerationParent.position, Quaternion.identity, cardsGenerationParent);
        card.GetComponent<CardInitialSetup>().Setup(cardInfo);
        return card;
    }

    #if UNITY_EDITOR
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
    #endif

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

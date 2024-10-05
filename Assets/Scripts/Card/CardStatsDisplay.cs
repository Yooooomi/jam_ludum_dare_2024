using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardStatsDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI hpText;
    [SerializeField]
    private TextMeshProUGUI manaText;
    [SerializeField]
    private TextMeshProUGUI attackText
;
    private CardBehavior card;

    void Start()
    {
        card = GetComponent<CardBehavior>();
        var stats = card.GetCardStats();
        manaText.text = stats.mana.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        var stats = card.GetCardStats();
        attackText.text = stats.damage.ToString();
        if (stats.health == stats.maxHealth)
        {
            hpText.text = stats.health.ToString();
        }
        else
        {
            hpText.text = stats.health.ToString() + "/" + stats.maxHealth.ToString();
        }
    }
}

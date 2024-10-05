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
        manaText.text = card.stats.mana.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        attackText.text = card.stats.damage.ToString();
        if (card.stats.health == card.stats.maxHealth) {
            hpText.text = card.stats.health.ToString();
        } else {
            hpText.text = card.stats.health.ToString() + "/" + card.stats.maxHealth.ToString();
        }
    }
}

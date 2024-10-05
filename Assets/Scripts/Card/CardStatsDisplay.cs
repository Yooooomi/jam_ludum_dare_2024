using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardStatsDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI hpText;
    [SerializeField]
    private Color hpBoostedColor;
    [SerializeField]
    private TextMeshProUGUI manaText;
    [SerializeField]
    private TextMeshProUGUI attackText;
    [SerializeField]
    private Color damageBoostedColor;
    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private float outlineWidth;

    private CardBehavior card;
    

    void Start()
    {
        card = GetComponent<CardBehavior>();
        card.onStatChanged.AddListener(UpdateStats);
        UpdateStats();
    }

    private void UpdateText(TextMeshProUGUI text) {
        text.outlineWidth = outlineWidth;
    }

    private void UpdateStats()
    {
        var stats = card.GetCardStats();
        manaText.text = stats.mana.ToString();
        manaText.outlineWidth = outlineWidth;
        attackText.text = stats.damage.ToString();
        attackText.color = card.IsDamageBuffed() ? damageBoostedColor : defaultColor;
        if (stats.health == stats.maxHealth)
        {
            hpText.text = stats.health.ToString();
        }
        else
        {
            hpText.text = stats.health.ToString() + "/" + stats.maxHealth.ToString();
        }
        hpText.color = card.IsHealthBuffed() ? hpBoostedColor : defaultColor;
        hpText.outlineWidth = outlineWidth;

        UpdateText(manaText);
        UpdateText(hpText);
        UpdateText(attackText);
    }
}

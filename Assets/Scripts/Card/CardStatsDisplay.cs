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
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI descText;
    [SerializeField]

    private CardBehavior card;

    void Start()
    {
        card = GetComponent<CardBehavior>();
        DelayedGameBridge.instance.onCardStatChange.AddListener(onDamageTaken);
        UpdateStats();
        nameText.text = card.card.info.name;
        UpdateText(nameText);
        descText.text = card.card.info.description;
        UpdateText(descText);
    }

    private void onDamageTaken(GameCard card)
    {
        if (this.card.card != card)
        {
            return;
        }
        UpdateStats();
    }

    private void UpdateText(TextMeshProUGUI text)
    {
        text.outlineWidth = outlineWidth;
    }

    private void UpdateStats()
    {
        var stats = card.card.GetCardStats();
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

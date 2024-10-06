using System.Collections;
using TMPro;
using UnityEngine;

public class CardStatsDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private Color healthBuffedColor;

    [SerializeField]
    private TextMeshProUGUI damageText;
    [SerializeField]
    private Color damageBuffedColor;

    [SerializeField]
    private TextMeshProUGUI manaText;

    [SerializeField]
    private Color defaultColor;

    [SerializeField]
    private float outlineWidth;

    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private CardBehavior card;

    private float defaultTextScale;
    private float defaultZ;

    void Start()
    {
        DelayedGameBridge.instance.onCardStatChange.AddListener(OnStatChange);

        card = GetComponent<CardBehavior>();

        damageText.outlineWidth = outlineWidth;
        manaText.outlineWidth = outlineWidth;
        healthText.outlineWidth = outlineWidth;

        defaultTextScale = damageText.transform.localScale.x;
        defaultZ = damageText.transform.localPosition.z;

        var stats = card.card.GetCardStats();
        damageText.text = stats.damage.ToString();
        healthText.text = stats.health.ToString();
        manaText.text = stats.mana.ToString();

        nameText.text = card.card.info.name;
        descriptionText.text = card.card.info.description;
    }

    private IEnumerator EnumeratorAnimateText(TextMeshProUGUI text, string newText)
    {
        var start = Time.time;
        var duration = 0.5f;
        var added = defaultTextScale * 1.5f;
        var zAdded = -0.1f;
        text.text = newText;
        while (start + duration > Time.time)
        {
            var ratio = (Time.time - start) / duration;
            text.transform.localScale = (defaultTextScale + ratio * added) * Vector3.one;
            var localPosition = text.transform.localPosition;
            localPosition.z = defaultZ + ratio * zAdded;
            text.transform.localPosition = localPosition;
            yield return new WaitForEndOfFrame();
        }
        text.transform.localScale = (defaultTextScale + added) * Vector3.one;
        start = Time.time;
        while (start + duration > Time.time)
        {
            var ratio = (Time.time - start) / duration;
            text.transform.localScale = (defaultTextScale + added - added * ratio) * Vector3.one;
            var localPosition = text.transform.localPosition;
            localPosition.z = defaultZ + zAdded - ratio * zAdded;
            text.transform.localPosition = localPosition;
            yield return new WaitForEndOfFrame();
        }
        text.transform.localScale = defaultTextScale * Vector3.one;
    }

    private void AnimateText(TextMeshProUGUI text, string newText)
    {
        StopAllCoroutines();
        StartCoroutine(EnumeratorAnimateText(text, newText));
    }

    private void OnStatChange(GameCard card, GameCardStats diff)
    {
        if (this.card.card != card)
        {
            return;
        }
        var stats = card.GetCardStats();
        if (diff.damage != 0)
        {
            AnimateText(damageText, card.GetCardStats().damage.ToString());
        }
        if (diff.mana != 0)
        {
            AnimateText(manaText, card.GetCardStats().mana.ToString());
        }
        if (diff.health != 0)
        {
            var newText = stats.health == stats.maxHealth ?
                healthText.text = stats.health.ToString() :
                stats.health.ToString() + "/" + stats.maxHealth.ToString();
            AnimateText(healthText, newText);
        }
        UpdateBuffed();
    }

    private void UpdateBuffed()
    {
        damageText.color = card.IsDamageBuffed() ? damageBuffedColor : defaultColor;
        healthText.color = card.IsHealthBuffed() ? healthBuffedColor : defaultColor;
    }
}

using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth;
    [HideInInspector]
    public int health;
    public int maxMana;
    [HideInInspector]
    public int mana;

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
        mana = maxMana;
        health = maxHealth;
    }

    private void OnPlaced(CardBehavior card)
    {
        if (card.playerId != player.id)
        {
            return;
        }
        mana -= card.stats.mana;
    }

    private void OnTurnBegin(int playerId)
    {
        if (player.id != playerId)
        {
            return;
        }
        mana = maxMana;
    }

    public bool ConsumeMana(int count)
    {
        if (count > mana)
        {
            return false;
        }
        mana -= count;
        return true;
    }
}

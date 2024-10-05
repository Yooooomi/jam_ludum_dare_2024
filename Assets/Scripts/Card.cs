using UnityEngine;

[System.Serializable]
public class Card
{
    public string name;
    public int maxHealth;
    public int damage;
    public int health;

    public Card()
    {
        health = maxHealth;
    }

    public bool Attack(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        return health < 0;
    }

    public void Heal(int heal)
    {
        health = Mathf.Clamp(health + heal, 0, maxHealth);
    }
}

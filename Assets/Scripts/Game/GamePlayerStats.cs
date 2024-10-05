public class GamePlayerStats
{
    public int maxHealth;
    public int health;
    public int maxMana;
    public int mana;

    public bool LoseHealth(int damage)
    {
        health -= damage;
        return health <= 0;
    }
}

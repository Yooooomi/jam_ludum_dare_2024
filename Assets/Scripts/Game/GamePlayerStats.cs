public class GamePlayerStats
{
    public int maxHealth = 30;
    public int health = 30;
    public int maxMana = 3;
    public int mana = 3;

    public bool LoseHealth(int damage)
    {
        health -= damage;
        return health <= 0;
    }
}

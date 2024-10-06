using UnityEngine;
using UnityEngine.SceneManagement;

public class Entrypoint : MonoBehaviour
{
    [SerializeField]
    private CardsCatalog catalog;

    private void Awake()
    {
        GameState.InitGameState(catalog);
        DelayedGameBridge.instance.onHeroStatChange.AddListener(OnHeroStatChange);
    }

    private void Start()
    {
        GameState.instance.StartGame();
    }

    private void OnHeroStatChange(GamePlayer player)
    {
        if (player.stats.health > 0)
        {
            return;
        }
        var scene = SceneManager.GetActiveScene();
        DelayedGameBridge.instance.Clear();
        SceneManager.LoadScene(scene.buildIndex);
    }
}

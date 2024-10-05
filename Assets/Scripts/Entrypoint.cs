using UnityEngine;

public class Entrypoint : MonoBehaviour
{
    [SerializeField]
    private CardsCatalog catalog;

    private void Awake()
    {
        GameState.InitGameState(catalog);
    }
}

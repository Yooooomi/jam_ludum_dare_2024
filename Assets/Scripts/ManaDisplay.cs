using System.Collections.Generic;
using UnityEngine;

public class ManaDisplay : MonoBehaviour
{
    [SerializeField]
    private Color disabledColor;
    [SerializeField]
    private List<Renderer> manaGems;

    private Color baseColor;
    private PlayerInstance player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerInstance>();
        DelayedGameBridge.instance.onHeroStatChange.AddListener(OnPlaced);
        baseColor = manaGems[0].sharedMaterial.color;
    }

    private void OnPlaced(GamePlayer player)
    {
        ComputeMana();
    }

    private void OnTurnEnd(int playerId)
    {
        ComputeMana();
    }

    private void ComputeMana()
    {
        var stats = GameState.instance.GetPlayer(player.player.id).stats;
        for (int i = 0; i < stats.mana; i += 1)
        {
            manaGems[i].material.color = baseColor;
        }
        for (int i = stats.mana; i < stats.maxMana; i += 1)
        {
            manaGems[i].material.color = disabledColor;
        }
    }
}

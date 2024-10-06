using UnityEngine;

public class HeroStatsDisplay : MonoBehaviour
{
  [SerializeField]
  private TMPro.TextMeshProUGUI health;
  private PlayerInstance player;

  private void Awake()
  {
    player = GetComponentInParent<PlayerInstance>();
    health.text = player.player.stats.health.ToString();
    DelayedGameBridge.instance.onHeroStatChange.AddListener(OnHeroStatChange);
  }

  private void OnHeroStatChange(GamePlayer hit)
  {
    if (hit.id != player.player.id)
    {
      return;
    }
    health.text = hit.stats.health.ToString();
  }
}
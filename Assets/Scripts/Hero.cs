using UnityEngine;
using UnityEngine.Events;

public class Hero : MonoBehaviour
{
    private PlayerInstance player;
    public UnityEvent onClick = new();

    private void Awake()
    {
        player = GetComponentInParent<PlayerInstance>();
    }

    public void Click()
    {
        onClick.Invoke();
    }
}

using UnityEngine;

public class Pop : MonoBehaviour
{
    public static Pop instance;
    [SerializeField]
    private float lifetime;
    [SerializeField]
    private float speed;
    public GameObject number;

    private void Awake()
    {
        instance = this;
    }

    public void PopElement(GameObject instantiated)
    {
        instantiated.transform.position += Vector3.up * 0.2f;
        var lifecycle = instantiated.AddComponent<PopLifecycle>();
        lifecycle.Setup(lifetime, speed);
    }
}

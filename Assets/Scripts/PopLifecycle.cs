using UnityEngine;

public class PopLifecycle : MonoBehaviour
{
    private float birth;
    private float time;
    private float speed;

    private void Awake()
    {
        birth = Time.time;
    }

    public void Setup(float time, float speed)
    {
        this.time = time;
        this.speed = speed;
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.forward;
        if (birth + time < Time.time)
        {
            Destroy(gameObject);
        }
    }
}

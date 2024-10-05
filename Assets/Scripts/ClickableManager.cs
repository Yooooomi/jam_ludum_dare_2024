using UnityEngine;

public class ClickableManager : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        var clicked = Input.GetMouseButtonDown(0);
        if (!clicked)
        {
            return;
        }
        var viewport = cam.ScreenToViewportPoint(Input.mousePosition);
        var ray = cam.ViewportPointToRay(viewport);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f);
        if (!Physics.Raycast(ray, out var hit))
        {
            return;
        }
        if (!hit.collider.gameObject.TryGetComponent<Clickable>(out var clickable))
        {
            return;
        }
        clickable.Click();
    }
}

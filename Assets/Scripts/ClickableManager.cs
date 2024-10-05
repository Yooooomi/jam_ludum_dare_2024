using System.Collections;
using System.Collections.Generic;
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
        var ray = cam.ViewportPointToRay(Input.mousePosition);
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

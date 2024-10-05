using UnityEngine;
using UnityEngine.Events;

public class Clickable : MonoBehaviour
{
    public UnityEvent OnClick = new();

    public void Click()
    {
        OnClick.Invoke();
    }
}

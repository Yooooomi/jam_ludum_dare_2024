using UnityEngine;

public class PopText : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI text;

    public void Setup(string text)
    {
        this.text.text = text;
    }
}

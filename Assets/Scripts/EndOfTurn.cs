using UnityEngine;



public class EndOfTurn : MonoBehaviour
{
    [SerializeField]
    private Transform arrowTransform;
    [SerializeField]
    private Transform endOfTurnButton;
    [SerializeField]
    private float arrowAnimationSpeed;
    [SerializeField]
    private float buttonAnimationSpeed;
    [SerializeField]
    private float buttonPressedOffset;

    private float startedAtButtonClick = 0;
    private float startedAtNextTurn = 0;

    private void Start()
    {
        DelayedGameBridge.instance.onTurnEnd.AddListener(OnTurnEnd);
    }

    public void OnClick()
    {
        if (startedAtButtonClick + buttonAnimationSpeed > Time.time)
        {
            return;
        }
        startedAtButtonClick = Time.time;
        GameState.instance.EndTurn();
    }

    private void OnTurnEnd(int unused_player_id)
    {
        startedAtNextTurn = Time.time;
    }

    bool IsP1Turn() {
        return GameState.instance.MyTurn(0);
    }

    void UpdateButton() {
        float halfAnimation = buttonAnimationSpeed / 2;
        bool buttonFirstHalfway = startedAtButtonClick - Time.time + halfAnimation > 0;
        float buttonTarget = buttonFirstHalfway ? -buttonPressedOffset : 0;
        float buttonOriginalPos = buttonFirstHalfway ? 0 : -buttonPressedOffset;
        float buttonPos = Mathf.Lerp(buttonOriginalPos, buttonTarget, Mathf.Clamp01((Time.time - startedAtButtonClick - (buttonFirstHalfway ? 0 : halfAnimation)) / halfAnimation));
        endOfTurnButton.localPosition = new Vector3(0, buttonPos, 0);
    }

    void UpdateArrow() {
        Quaternion p2Rotation = Quaternion.Euler(0, -180, 0);
        Quaternion arrowRotation = Quaternion.Lerp(IsP1Turn() ? p2Rotation : Quaternion.identity, IsP1Turn() ? Quaternion.identity : p2Rotation, Mathf.Clamp01((Time.time - startedAtNextTurn) / arrowAnimationSpeed));
        arrowTransform.rotation = arrowRotation;
    }

    void Update()
    {
        if (startedAtButtonClick != 0) {
            UpdateButton();
        }
        if (startedAtNextTurn != 0) {
            UpdateArrow();
        }
    }
}

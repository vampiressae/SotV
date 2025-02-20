using UnityEngine;

public class PopupGameOver : UIPopupScaledTextBase
{
    [Space]
    [SerializeField] private float _delay;

    protected override void Start()
    {
        base.Start();
        Invoke(nameof(Close), _delay + _duration + 2);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        CancelInvoke(nameof(Close));
    }

    public void Close() => SceneController.Instance.LoadScene("Main");
}

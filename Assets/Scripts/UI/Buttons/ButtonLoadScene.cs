using UnityEngine;

public class ButtonLoadScene : UIButtonBase
{
    [SerializeField] private string _scene;

    protected override void Click() => SceneController.Instance.LoadScene(_scene);
}

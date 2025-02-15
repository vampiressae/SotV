using Vamporium.UI;

public class ButtonQuit : UIButtonBase
{
    protected override void Click() 
        => UIManager.ShowMessage().Init("Quit", "Are you sure you want to quit?").SetButtonsYesNo(Quit);

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }
}

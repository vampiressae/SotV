using UnityEngine;

namespace Vamporium.UI
{
    public class UIAgent : MonoBehaviour
    {
        public void ShowUI(UITag tag) => UIManager.Show(tag);
        public void ShowPreviousScreen() => UIManager.ShowPreviousScreen();

        public void HideUI(UITag tag) => UIManager.Hide(tag);
        public void HidePopups() => UIManager.HidePopups();
    }
}

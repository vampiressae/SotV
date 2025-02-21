using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class TestersToolsFight : MonoBehaviour
{
    [SerializeField] private Button _win, _lose;

    private void OnEnable()
    {
        _win.onClick.AddListener(Win);
        _lose.onClick.AddListener(Lose);
    }

    private void OnDisable()
    {
        _win.onClick.RemoveListener(Win);
        _lose.onClick.RemoveListener(Lose);
    }

    private void Win() => FightController.Enemies.ForEach(enemy => enemy.Info.Kill());
    private void Lose() => FightController.Player.Info.Kill();
}

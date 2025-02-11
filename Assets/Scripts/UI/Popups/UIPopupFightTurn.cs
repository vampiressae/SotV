using System.Collections;
using UnityEngine;
using TMPro;
using Vamporium.UI;

public class UIPopupFightTurn : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _delay = 1.5f;

    public void Init(string text) => _text.text = text;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_delay);
        GetComponent<UIPopup>().Hide();
    }
}

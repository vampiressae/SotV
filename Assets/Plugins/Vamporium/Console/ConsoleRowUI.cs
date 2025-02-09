using TMPro;
using UnityEngine;

namespace Console
{
    public class ConsoleRowUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private ConsoleData _data;

        public void Init(ConsoleData data)
        {
            _data = data;
            _text.text = _data.Text;
        }
    }
}

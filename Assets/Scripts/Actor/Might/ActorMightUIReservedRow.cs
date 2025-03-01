using UnityEngine;
using TMPro;

namespace Actor
{
    public class ActorMightUIReservedRow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name, _value;

        public void Init(string name, int value)
        {
            _name.text = name;
            _value.text = value.ToString();
        }
    }
}

using Sirenix.OdinInspector;
using UnityEngine;

namespace Entity
{
    [System.Serializable]
    public class StatData
    {
        [SerializeField, HideLabel, HorizontalGroup(50)] protected int _amount;
        [SerializeField, HideLabel, HorizontalGroup] protected StatInfo _info;

        public int Amount => _amount;
        public StatInfo Info => _info;
        public object Source { get; private set; }

        public float InfluencedAmount => Amount * Info.Influence;

        public StatData(StatInfo info) => _info = info;
        public StatData(StatInfo info, int amount) : this(info) => _amount = amount;
        public StatData(object source, StatInfo info, int amount) : this(info, amount) => Source = source;

        public void SetAmount(int amount) => _amount = amount;

        public void AddAmount(int amount)
        {
            _amount += amount;
            if (_info.Max > 0 && _amount > _info.Max)
                _amount = _info.Max;
            //else if (_amount < 0) _amount = 0;
        }
    }
}


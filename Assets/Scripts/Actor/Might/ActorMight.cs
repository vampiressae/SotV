using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    [Serializable]
    public partial class ActorMight
    {
        public enum MightTypeEffect { None, Reserved, Missing, Recoverable, Regen, Max }
        public enum MightType { None, Reserved, Preview, Missing, Consumed, Regen, Available, Max }

        public event Action OnAnyValueChanged;
        public event Action<int> OnReservedChanged;
        public event Action<int> OnPreviewChanged;
        public event Action<int, IActorMightMissing> OnMissingChanged;
        public event Action<int> OnConsumedChanged;

        [SerializeField] private int _max = 100;
        [SerializeField] private int _regen = 10;

        private readonly Dictionary<IActorMightReserver, int> _reserved = new();
        private readonly Dictionary<IActorMightPreviewer, int> _preview = new();

        public IReadOnlyDictionary<IActorMightReserver, int> AllReserved => _reserved;
        public IReadOnlyDictionary<IActorMightPreviewer, int> AllPreview => _preview;

        private int _missing, _consumed;

        [ReadOnlyInPlayMode] public int Max => GetModifiedValue(MightType.Max, _max);
        [ReadOnlyInPlayMode] public int Reserved => _reserved.Values.Sum();
        [ReadOnlyInPlayMode] public int Preview => _preview.Values.Sum();
        [ReadOnlyInPlayMode] public int Missing => _missing;
        [ReadOnlyInPlayMode] public int Consumed => _consumed;

        public bool Alive => Available > 0;
        public bool Dead => !Alive;

        [ReadOnlyInPlayMode] public int Available => Max - Reserved - Missing - Consumed;

        [ReadOnlyInPlayMode] private readonly List<MightModifier> _multipliers = new();
        [ReadOnlyInPlayMode] private readonly List<MightModifier> _additions = new();

        public int GetValueByType(MightType type) => type switch
        {
            MightType.Reserved => Reserved,
            MightType.Preview => Preview,
            MightType.Missing => Missing,
            MightType.Consumed => Consumed,
            MightType.Available => Available,
            MightType.Max => Max,
            _ => 0,
        };

        public void RegenerateConsumed(bool full = false) => RemoveConsumedValue(full ? _consumed : _regen);
    }
}

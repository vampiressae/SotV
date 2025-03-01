using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public interface IActorMightDictionaryKey
    {
        string Name { get; }
    }
    public interface IActorMightReserver : IActorMightDictionaryKey { }
    public interface IActorMightPreviewer : IActorMightDictionaryKey { }
    public interface IActorMightMultiplier : IActorMightDictionaryKey { }
    public interface IActorMighAddition : IActorMightDictionaryKey { }

    [Serializable]
    public partial class ActorMight
    {
        public enum MightTypeEffect { None, Reserved, Missing, Recoverable, Regen, Max }
        public enum MightType { None, Reserved, Preview, Missing, Recoverable, Regen, Available, Max }

        public event Action OnAnyValueChanged;
        public event Action<int> OnReservedChanged;
        public event Action<int> OnPreviewChanged;
        public event Action<int> OnMissingChanged;
        public event Action<int> OnRecoverableChanged;

        [SerializeField] private int _max = 100;
        [SerializeField] private int _regen = 10;

        private readonly Dictionary<IActorMightReserver, int> _reserved = new();
        private readonly Dictionary<IActorMightPreviewer, int> _preview = new();

        public IReadOnlyDictionary<IActorMightReserver, int> AllReserved => _reserved;
        public IReadOnlyDictionary<IActorMightPreviewer, int> AllPreview => _preview;

        private int _missing, _recoverable;

        [ReadOnlyInPlayMode] public int Max => GetModifiedValue(MightType.Max, _max);
        [ReadOnlyInPlayMode] public int Reserved => _reserved.Values.Sum();
        [ReadOnlyInPlayMode] public int Preview => _preview.Values.Sum();
        [ReadOnlyInPlayMode] public int Missing => _missing;
        [ReadOnlyInPlayMode] public int Recoverable => _recoverable;

        public bool Alive => Available > 0;
        public bool Dead => !Alive;

        [ReadOnlyInPlayMode] public int Available => Max - Reserved - Missing - Recoverable;

        [ReadOnlyInPlayMode] private readonly Dictionary<MightType, Dictionary<IActorMightMultiplier, int>> _multipliers = new();
        [ReadOnlyInPlayMode] private readonly Dictionary<MightType, Dictionary<IActorMighAddition, int>> _additions = new();

        public IReadOnlyDictionary<MightType, Dictionary<IActorMightMultiplier, int>> Multipliers => _multipliers;
        public IReadOnlyDictionary<MightType, Dictionary<IActorMighAddition, int>> Additions => _additions;

        public int GetValueByType(MightType type) => type switch
        {
            MightType.Reserved => Reserved,
            MightType.Preview => Preview,
            MightType.Missing => Missing,
            MightType.Recoverable => Recoverable,
            MightType.Available => Available,
            MightType.Max => Max,
            _ => 0,
        };

        public void Regen(bool full = false) => RemoveRecoverableValue(full ? _recoverable : _regen);
    }
}

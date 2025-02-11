using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Actor
{
    [Serializable]
    public class ActorMight
    {
        public enum MightType { None, Reserved, Preview, Missing, Recoverable, Regen, Available, Max }

        public event Action OnAnyValueChanged;
        public event Action<int> OnReservedChanged;
        public event Action<int> OnPreviewChanged;
        public event Action<int> OnMissingChanged;
        public event Action<int> OnRecoverableChanged;

        [SerializeField] private int _max = 100;
        [SerializeField] private int _regen = 10;

        private Dictionary<object, int> _reserved = new();
        private Dictionary<object, int> _preview = new();

        private int _missing, _recoverable;

        [ReadOnlyInPlayMode] public int Max => GetModifiedValue(MightType.Max, _max);
        [ReadOnlyInPlayMode] public int Reserved => _reserved.Values.Sum();
        [ReadOnlyInPlayMode] public int Preview => _preview.Values.Sum();
        [ReadOnlyInPlayMode] public int Missing => _missing;
        [ReadOnlyInPlayMode] public int Recoverable => _recoverable;

        [ReadOnlyInPlayMode] public int Available => Max - Reserved - Missing - Recoverable;

        [ReadOnlyInPlayMode] private readonly Dictionary<MightType, Dictionary<object, int>> _multipliers = new();
        [ReadOnlyInPlayMode] private readonly Dictionary<MightType, Dictionary<object, int>> _additions = new();

        public IReadOnlyDictionary<MightType, Dictionary<object, int>> Multipliers => _multipliers;
        public IReadOnlyDictionary<MightType, Dictionary<object, int>> Additions => _additions;

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

        public void AddReservedValue(object source, int value) => AddValue(_reserved, source, GetModifiedValue(MightType.Reserved, value), OnReservedChanged);
        public void AddPreviewValue(object source, int value) => AddValue(_preview, source, value, OnPreviewChanged);
        public void RemoveReservedValue(object source) => RemoveValue(_reserved, source, OnReservedChanged);
        public void RemovePreviewValue(object source) => RemoveValue(_preview, source, OnPreviewChanged);

        private void AddValue(Dictionary<object, int> dictionary, object source, int value, Action<int> action)
        {
            dictionary[source] = value;
            action?.Invoke(value);
            OnAnyValueChanged?.Invoke();
        }

        private void RemoveValue(Dictionary<object, int> dictionary, object source, Action<int> action)
        {
            if (!dictionary.ContainsKey(source)) return;

            var value = dictionary[source];
            dictionary.Remove(source);

            action?.Invoke(value);
            OnAnyValueChanged?.Invoke();
        }

        public void AddMissingValue(int value)
        {
            value = GetModifiedValue(MightType.Missing, value);
            _missing += value;

            OnMissingChanged?.Invoke(value);
            OnAnyValueChanged?.Invoke();
        }

        public void RemoveMissingValue(int value)
        {
            _missing = Mathf.Max(_missing - value, 0);

            OnMissingChanged?.Invoke(-value);
            OnAnyValueChanged?.Invoke();
        }

        public void AddRecoverableValue(int value)
        {
            value = GetModifiedValue(MightType.Recoverable, value);
            _recoverable += value;

            OnRecoverableChanged?.Invoke(value);
            OnAnyValueChanged?.Invoke();
        }

        public void RemoveRecoverableValue(int value)
        {
            value = GetModifiedValue(MightType.Regen, value);
            _recoverable = Mathf.Max(_recoverable - value, 0);

            OnRecoverableChanged?.Invoke(value);
            OnAnyValueChanged?.Invoke();
        }

        public void Regen() => RemoveRecoverableValue(_regen);

        public void AddMultiplier(MightType type, object source, int value) => AddToDictionary(_multipliers, type, source, value);
        public void RemoveMultiplier(MightType type, object source) => RemoveFromDictionary(_multipliers, type, source);
        public void AddAddition(MightType type, object source, int value) => AddToDictionary(_additions, type, source, value);
        public void RemoveAddition(MightType type, object source) => RemoveFromDictionary(_additions, type, source);

        private int GetModifiedValue(MightType type, int value)
        {
            var multiplier = _multipliers.TryGetValue(type, out var multipliers) ? multipliers.Values.Sum() : 0;
            var addition = _additions.TryGetValue(type, out var additions) ? additions.Values.Sum() : 0;
            return Mathf.RoundToInt(value + (value * multiplier * 0.01f) + addition);
        }

        private void AddToDictionary(Dictionary<MightType, Dictionary<object, int>> dictionary, MightType type, object source, int value)
        {
            if (!dictionary.TryGetValue(type, out var list))
            {
                list = new();
                dictionary.Add(type, list);
            }

            dictionary[type][source] = value;
            OnAnyValueChanged?.Invoke();
        }

        private void RemoveFromDictionary(Dictionary<MightType, Dictionary<object, int>> dictionary, MightType type, object source)
        {
            if (type == MightType.None) return;
            if (!dictionary.ContainsKey(type)) return;
            if (!dictionary[type].ContainsKey(source)) return;

            dictionary[type].Remove(source);
            OnAnyValueChanged?.Invoke();

            ClearNullDictionaryEntries(dictionary);
        }

        private void ClearNullDictionaryEntries(Dictionary<MightType, Dictionary<object, int>> dictionary)
        {
            var empties = dictionary.Where(pair => pair.Value.Count == 0).Select(pair => pair.Key).ToList();
            if (empties.Count() > 0) foreach (var empty in empties) dictionary.Remove(empty);
        }
    }
}

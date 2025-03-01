using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Actor
{
    public partial class ActorMight
    {
        public void AddMultiplier(MightType type, IActorMightMultiplier source, int value) => AddToDictionary(_multipliers, type, source, value);
        public void RemoveMultiplier(MightType type, IActorMightMultiplier source) => RemoveFromDictionary(_multipliers, type, source);
        public void AddAddition(MightType type, IActorMighAddition source, int value) => AddToDictionary(_additions, type, source, value);
        public void RemoveAddition(MightType type, IActorMighAddition source) => RemoveFromDictionary(_additions, type, source);

        private int GetModifiedValue(MightType type, int value)
        {
            var multiplier = _multipliers.TryGetValue(type, out var multipliers) ? multipliers.Values.Sum() : 0;
            var addition = _additions.TryGetValue(type, out var additions) ? additions.Values.Sum() : 0;
            return Mathf.RoundToInt(value + (value * multiplier * 0.01f) + addition);
        }

        private void AddToDictionary<T>(Dictionary<MightType, Dictionary<T, int>> dictionary, MightType type, T source, int value) where T : IActorMightDictionaryKey
        {
            if (!dictionary.TryGetValue(type, out var list))
            {
                list = new();
                dictionary.Add(type, list);
            }

            dictionary[type][source] = value;
            OnAnyValueChanged?.Invoke();
        }

        private void RemoveFromDictionary<T>(Dictionary<MightType, Dictionary<T, int>> dictionary, MightType type, T source) where T : IActorMightDictionaryKey
        {
            if (type == MightType.None) return;
            if (!dictionary.ContainsKey(type)) return;
            if (!dictionary[type].ContainsKey(source)) return;

            dictionary[type].Remove(source);
            OnAnyValueChanged?.Invoke();

            ClearNullDictionaryEntries(dictionary);
        }

        private void ClearNullDictionaryEntries<T>(Dictionary<MightType, Dictionary<T, int>> dictionary) where T : IActorMightDictionaryKey
        {
            var empties = dictionary.Where(pair => pair.Value.Count == 0).Select(pair => pair.Key).ToList();
            if (empties.Count() > 0) foreach (var empty in empties) dictionary.Remove(empty);
        }
    }
}

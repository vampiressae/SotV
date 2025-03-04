using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Actor
{
    public partial class ActorMight
    {
        [Serializable]
        private class MightModifierValue
        {
            [HideLabel, HorizontalGroup] public IActorMightDictionaryKey Source;
            [HideLabel, HorizontalGroup(50)] public int Value;
        }

        private class MightModifierModule
        {
            [HideLabel] public IActorMightDictionaryKey Key;
            public List<MightModifierValue> Values = new();
            public int Sum() => Values?.Sum(value => value.Value) ?? 0;
        }

        private class MightModifier
        {
            [HideLabel] public MightType Type;
            public List<MightModifierModule> Modules = new();
            public int Sum(IActorMightDictionaryKey key) => Modules?.Sum(value => value.Key == key ? value.Sum() : 0) ?? 0;
        }

        public void AddMultiplier(MightType type, IActorMightMultiplier source, int value) => AddMultiplier(type, source, null, value);
        public void AddMultiplier(MightType type, IActorMightMultiplier source, IActorMightDictionaryKey key, int value)
            => AddToDictionary(_multipliers, type, source, key, value);

        public void RemoveMultiplier(MightType type, IActorMightMultiplier source, int value) => RemoveMultiplier(type, source, null, value);
        public void RemoveMultiplier(MightType type, IActorMightMultiplier source, IActorMightDictionaryKey key, int value)
            => RemoveFromDictionary(_multipliers, type, source, key, value);

        public void AddAddition(MightType type, IActorMighAddition source, int value) => AddAddition(type, source, null, value);
        public void AddAddition(MightType type, IActorMighAddition source, IActorMightDictionaryKey key, int value)
              => AddToDictionary(_additions, type, source, key, value);

        public void RemoveAddition(MightType type, IActorMighAddition source, int value) => RemoveAddition(type, source, null, value);
        public void RemoveAddition(MightType type, IActorMighAddition source, IActorMightDictionaryKey key, int value)
                => RemoveFromDictionary(_additions, type, source, key, value);

        private int GetModifiedValue(MightType type, int value, IActorMightDictionaryKey key = null)
        {
            var multiplier = GetModifiedValue(_multipliers, type, key);
            var addition = GetModifiedValue(_additions, type, key);
            return Mathf.RoundToInt(value + (value * multiplier * 0.01f) + addition);
        }

        private int GetModifiedValue(List<MightModifier> list, MightType type, IActorMightDictionaryKey key)
        {
            var modifier = list.Where(module => module.Type == type).FirstOrDefault();
            if (modifier == null) return 0;

            var module = modifier.Modules.Where(module => module.Key == key).FirstOrDefault();
            return module == null ? 0 : module.Sum();
        }

        private void AddToDictionary<T>(List<MightModifier> list, MightType type, T source, T key, int value) where T : IActorMightDictionaryKey
        {
            var modifier = GetElementAndAddIfMissing(list, m => m.Type == type, m => m.Type = type);
            var module = GetElementAndAddIfMissing(modifier.Modules, m => CompareDictionaryKeys(m.Key, key), m => m.Key = key);
            var result = GetElementAndAddIfMissing(module.Values, v => CompareDictionaryKeys(v.Source, source), v => v.Source = source);

            result.Value += value;
            OnAnyValueChanged?.Invoke();
        }

        private void RemoveFromDictionary<T>(List<MightModifier> list, MightType type, T source, T key, int value) where T : IActorMightDictionaryKey
        {
            var modifier = GetElementAndAddIfMissing(list, m => m.Type == type);
            if (modifier == null) return;

            var module = GetElementAndAddIfMissing(modifier.Modules, m => CompareDictionaryKeys(m.Key, key));
            if (module == null) return;

            var result = GetElementAndAddIfMissing(module.Values, v => CompareDictionaryKeys(v.Source, source));
            if (result == null || result.Value < value) return;

            result.Value -= value;
            OnAnyValueChanged?.Invoke();
        }

        private bool CompareDictionaryKeys(IActorMightDictionaryKey a, IActorMightDictionaryKey b) => a == b;

        private T GetElementAndAddIfMissing<T>(List<T> list, Func<T, bool> findElement, Action<T> setElement = null) where T : new()
        {
            var element = list.Where(module => findElement(module)).FirstOrDefault();
            if (element == null && setElement != null)
            {
                element = new();
                setElement(element);
                list.Add(element);
            }
            return element;
        }
    }
}

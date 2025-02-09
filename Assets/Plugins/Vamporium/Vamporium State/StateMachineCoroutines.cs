using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VamporiumState
{
    public class StateMachineCoroutines : MonoBehaviour
    {
        private Dictionary<string, Coroutine> _coroutines = new();
        public IReadOnlyDictionary<string, Coroutine> Coroutines => _coroutines;

        public Coroutine DelayedAction(float delay, Action action)
            => StartCoroutine(Delay(delay, action));

        public void DelayedAction(string key, float delay, Action action)
        {
            CancelDelayedAction(key);
            _coroutines[key] = StartCoroutine(Delay(delay, action));
        }

        public void CancelDelayedAction(Coroutine coroutine)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }

        public void CancelDelayedAction(string key)
        {
            if (!_coroutines.TryGetValue(key, out var coroutine))
                StopCoroutine(coroutine);
            _coroutines.Remove(key);
        }

        public void CancelAllDelayedActions()
        {
            foreach (var coroutine in _coroutines)
                StopCoroutine(coroutine.Value);
            _coroutines.Clear();
        }

        private IEnumerator Delay(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}

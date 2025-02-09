using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VamporiumState
{
    public abstract partial class StateMachine<T> : MonoBehaviour, IStateMachine where T : IState
    {
        public event Func<StateMachine<T>, bool> ShouldStateChange;
        public event Action<StateMachine<T>> OnStateChange;

        [SerializeField] protected T _startingState;
        [SerializeField] protected T[] _states;

        [ShowInInspector, HideInEditorMode, ReadOnly] public T Current { get; private set; }
        [ShowInInspector, HideInEditorMode, ReadOnly] public T Previous { get; private set; }
        [ShowInInspector, HideInEditorMode, ReadOnly] public T Desired { get; private set; }
        [ShowInInspector, HideInEditorMode, ReadOnly] public float TimeInState => Time.time - _enterTime;

        public virtual string CurrentStateName => Current == null ? "NONE" : Current.Name;
        public StateMachineCoroutines Coroutines => GetCoroutinesSafely();

        private float _enterTime;
        private StateMachineCoroutines _coroutines;

        protected virtual void Update() => UpdateLogics(Current);
        protected virtual void FixedUpdate() => UpdatePhysics(Current);

        public abstract void UpdateLogics(T state);
        public abstract void UpdatePhysics(T state);

        protected abstract bool CanEnter(T state);
        protected abstract bool CanExit(T state);
        protected abstract void Enter(T state);
        protected abstract void Exit(T state);

        protected void Start()
        {
            if (_startingState != null)
                ChangeState(_startingState);
        }

        public void AdvanceState()
        {
            var current = Array.IndexOf(_states, Current) + 1;
            if (current < 0 || current >= _states.Length) return;

            ChangeState(_states[current]);
        }

        public void ChangeState<TState>() where TState : T
        {
            foreach (var state in _states)
            {
                if (state is not TState) continue;
                ChangeState(state);
                break;
            }
        }

        public void ChangeState(T state)
        {
            Desired = state;

            if (!InvokeShouldStateChange()) return;
            if (!CanEnter(state)) return;

            if (Current != null)
            {
                if (!CanExit(Current)) return;
                Exit(Current);
            }

            Desired = default;
            Previous = Current;
            Current = state;
            _enterTime = Time.time;

            Enter(Current);
            InvokeOnStateChange();
        }

        protected virtual void InvokeOnStateChange() => OnStateChange?.Invoke(this);
        protected virtual bool InvokeShouldStateChange() => ShouldStateChange == null || ShouldStateChange(this);

        private StateMachineCoroutines GetCoroutinesSafely()
        {
            if (_coroutines == null) _coroutines = gameObject.AddComponent<StateMachineCoroutines>();
            return _coroutines;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_startingState == null && _states != null && _states.Length > 0)
                _startingState = _states[0];
        }
#endif
    }
}

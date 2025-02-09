namespace VamporiumState.GO
{
    public class StateMachine : StateMachine<State>
    {
        public override void UpdateLogics(State state) => state.UpdateLogics(this);
        public override void UpdatePhysics(State state) => state.UpdatePhyics(this);

        protected override bool CanEnter(State state) => state.CanEnter(this);
        protected override bool CanExit(State state) => state.CanExit(this);
        protected override void Enter(State state) => state.Enter(this);
        protected override void Exit(State state) => state.Exit(this);

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            _states = GetComponentsInChildren<State>();
            base.OnValidate();
        }
#endif
    }
}

using UnityEngine;

namespace VamporiumState.GO
{
    public class StateDelay : State
    {
        [SerializeField] private float _autoAdvance = 1;

        public override void UpdateLogics(StateMachine machine)
        {
            base.UpdateLogics(machine);
            if (machine.TimeInState < _autoAdvance) return;

            machine.AdvanceState();
        }
    }
}

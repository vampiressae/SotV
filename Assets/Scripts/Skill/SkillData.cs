namespace Skills
{
    [System.Serializable]
    public class SkillData : Item<SkillInfo>
    {
        public int Level => _amount;

        public override bool IsEmpty => false;
        public override bool IsFull => false;
        public bool HasTimer => _timerMax > 0;

        private int _timerMax, _timer;

        public SkillData(int time) => AddTimer(time);

        public void AddTimer(int time)
        {
            _timerMax += time;
            _timer += time;
        }

        public virtual void TurnStart(ActionInfo actor, int turn)
        {
            if (_timerMax > 0)
            {
                _timer--;
                Info.Apply(actor);
            }
            if (_timerMax < 1 || _timer < 1) ResetTimer();
        }

        private void ResetTimer() => _timerMax = _timer = 0;

        public override void InvokeOnItemDataChanging() {/* not important now */}
        public override void InvokeOnItemDataChanged() {/* not important now */}
        public override void Swap(Item with) {/* not important now */}
    }
}

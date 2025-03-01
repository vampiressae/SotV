using UnityEngine;

namespace Actor
{
    public partial class ActorMight
    {
        public void AddReservedValue(IActorMightReserver source, int value)
        {
            _reserved[source] = value;
            OnReservedChanged?.Invoke(value);
            OnAnyValueChanged?.Invoke();
        }

        public void RemoveReservedValue(IActorMightReserver source)
        {
            if (!_reserved.ContainsKey(source)) return;
            var value = _reserved[source];
            _reserved.Remove(source);
            OnReservedChanged?.Invoke(value);
            OnAnyValueChanged?.Invoke();
        }

        public void AddPreviewValue(IActorMightPreviewer source, int value)
        {
            _preview[source] = value;
            OnPreviewChanged?.Invoke(value);
            OnAnyValueChanged?.Invoke();
        }

        public void RemovePreviewValue(IActorMightPreviewer source)
        {
            if (!_preview.ContainsKey(source)) return;
            var value = _preview[source];
            _preview.Remove(source);
            OnPreviewChanged?.Invoke(value);
            OnAnyValueChanged?.Invoke();
        }

        public void AddMissingValue(int value)
        {
            if (value == 0) return;
            value = GetModifiedValue(MightType.Missing, value);

            if (value < 1) value = 0;
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

        public void ResetMissingValue()
        {
            if (_missing == 0) return;

            var missing = _missing;
            _missing = 0;

            OnMissingChanged?.Invoke(missing);
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
    }
}

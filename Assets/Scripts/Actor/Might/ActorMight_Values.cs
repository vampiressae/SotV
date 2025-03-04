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

        public void AddMissingValue(int value, IActorMightMissing iMissing = null, IActorMightDictionaryKey key = null)
        {
            if (value == 0) return;
            value = GetModifiedValue(MightType.Missing, value, key);

            if (value < 1) value = 0;
            _missing += value;

            OnMissingChanged?.Invoke(value, iMissing);
            OnAnyValueChanged?.Invoke();
        }

        public void RemoveMissingValue(int value, IActorMightMissing iMissing = null)
        {
            _missing = Mathf.Max(_missing - value, 0);

            OnMissingChanged?.Invoke(-value, iMissing);
            OnAnyValueChanged?.Invoke();
        }

        public void ResetMissingValue()
        {
            if (_missing == 0) return;

            var missing = _missing;
            _missing = 0;

            OnMissingChanged?.Invoke(missing, null);
            OnAnyValueChanged?.Invoke();
        }   

        public void AddConsumedValue(int value)
        {
            value = GetModifiedValue(MightType.Consumed, value);
            _consumed += value;

            OnConsumedChanged?.Invoke(value);
            OnAnyValueChanged?.Invoke();
        }

        public void RemoveConsumedValue(int value)
        {
            value = GetModifiedValue(MightType.Regen, value);
            _consumed = Mathf.Max(_consumed - value, 0);

            OnConsumedChanged?.Invoke(value);
            OnAnyValueChanged?.Invoke();
        }
    }
}

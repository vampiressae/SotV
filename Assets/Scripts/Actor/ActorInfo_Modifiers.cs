using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using Affliction;
using Modifier;

namespace Actor
{
    public partial class ActorInfo
    {
        public void AddModifier(ModifierData data)
        {
            _modifiers.Add(data);
            data.OnModifierEvent(ModifierEvent.OnAdded, this);
        }

        private void InvokeModifierEvent(ModifierEvent e)
        {
            var removes = new List<ModifierData>();
            foreach (var modifier in _modifiers)
                if (modifier.OnModifierEvent(e, this))
                    removes.Add(modifier);
            RemoveExpiredModifiers(removes);
        }

        private void InvokeModifierEvent(ModifierEventInt e, ref int value)
        {
            var removes = new List<ModifierData>();
            foreach (var modifier in _modifiers)
                if (modifier.OnModifierEvent(e, this, ref value))
                    removes.Add(modifier);
            RemoveExpiredModifiers(removes);
        }

        private void RemoveExpiredModifiers(List<ModifierData> list)
        {
            foreach (var data in list)
            {
                _modifiers.Remove(data);
                data.OnModifierEvent(ModifierEvent.OnRemoved, this);
            }
        }

        public void AddAfflictions(IEnumerable<AfflictionData> datas)
            => datas.ForEach(info => AddAffliction(info));

        public void AddAffliction(AfflictionData data)
        {
            switch (data.GetAddMode())
            {
                case AfflictionAddMode.Unique:
                    _afflictions.Add(data.Clone());
                    break;

                case AfflictionAddMode.Stack:
                    var found = _afflictions.Where(affliction => affliction.Info.Name == data.Info.Name).FirstOrDefault();
                    if (found != null) found.AddApplies(data.Applies);
                    else _afflictions.Add(data.Clone());
                    break;
            }
            InvokeOnFlyingText().Init(data.Info.Name, data.Info.Color);
        }

        public void RemoveAffliction(AfflictionData data)
        {
            foreach (var affliction in _afflictions)
            {
                if (affliction != data) continue;
                _afflictions.Remove(affliction);
                break;
            }
        }

        public void RemoveAfflictions(IEnumerable<AfflictionData> datas)
            => datas.ForEach(affliction => RemoveAffliction(affliction));

        public void RemoveAffliction(AfflictionData data, bool all)
        {
            var removes = _afflictions.Where(affliction => affliction.Info.Name == data.Info.Name);
            foreach (var remove in removes)
            {
                _afflictions.Remove(remove);
                if (!all) break;
            }
        }

        public void InvokAfflictions(AfflictionMoment moment)
        {
            var removes = new List<AfflictionData>();
            foreach (var data in _afflictions)
                if (data.Afflict(moment, this))
                    removes.Add(data);
            RemoveAfflictions(removes);
        }
    }
}

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

        public void AddAfflictions(IEnumerable<AfflictionInfo> infos)
            => infos.ForEach(info => AddAffliction(info));

        public void AddAffliction(AfflictionInfo info)
        {
            switch (info.GetAddMode())
            {
                case AfflictionInfo.AddMode.Unique:
                    _afflictions.Add(Instantiate(info));
                    break;

                case AfflictionInfo.AddMode.Stack:
                    var found = _afflictions.Where(affliction => affliction.Name == info.Name).FirstOrDefault();
                    if (found) found.AddApplies(info.Applies);
                    else _afflictions.Add(Instantiate(info));
                    break;
            }
            InvokeOnFlyingText().Init(info.Name, info.Color);
        }

        public void RemoveAffliction(AfflictionInfo info)
        {
            foreach (var affliction in _afflictions)
            {
                if (affliction != info) continue;
                _afflictions.Remove(affliction);
                break;
            }
        }

        public void RemoveAfflictions(IEnumerable<AfflictionInfo> infos)
            => infos.ForEach(affliction => RemoveAffliction(affliction));


        public void RemoveAffliction(AfflictionInfo info, bool all)
        {
            var removes = _afflictions.Where(affliction => affliction.Name == info.Name);
            foreach (var remove in removes)
            {
                _afflictions.Remove(remove);
                if (!all) break;
            }
        }

        public void InvokAfflictions(AfflictionMoment moment)
        {
            var removes = new List<AfflictionInfo>();
            foreach (var modifier in _afflictions)
                if (modifier.Afflict(moment, this))
                    removes.Add(modifier);
            RemoveAfflictions(removes);
        }
    }
}

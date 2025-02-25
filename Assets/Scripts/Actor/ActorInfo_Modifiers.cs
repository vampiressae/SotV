using System.Collections.Generic;
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
    }
}

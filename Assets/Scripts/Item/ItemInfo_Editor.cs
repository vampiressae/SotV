#if UNITY_EDITOR
using Sirenix.OdinInspector;

namespace Items
{
    public partial class ItemInfo
    {
        private void ClearNullItemEffects()
        {
            for (int i = _effects.Count - 1; i >= 0; i--)
                if (_effects[i] == null)
                    _effects.RemoveAt(i);
        }

        public void AddItemEffect(ItemEffect effect)
        {
            ClearNullItemEffects();
            if (effect == null) return;

            var add = Instantiate(effect);
            add.name = name + " : " + effect.name;
            _effects.Add(add);
            UnityEditor.AssetDatabase.AddObjectToAsset(add, this);
        }

        private void RemoveItemEffect(ItemEffect remove)
        {
            ClearNullItemEffects();
            if (remove == null) return;

            _effects.Remove(remove);
            UnityEditor.AssetDatabase.RemoveObjectFromAsset(remove);
        }

        [Title("Editor", TitleAlignment = TitleAlignments.Centered)]
        [LabelText("AddEffect"), OnValueChanged(nameof(AddItemEffectInEditor)), ShowInInspector]
        private ItemEffect _addInEditor { get; set; }
        private void AddItemEffectInEditor()
        {
            AddItemEffect(_addInEditor);
            _addInEditor = null;
        }
    }
}
#endif

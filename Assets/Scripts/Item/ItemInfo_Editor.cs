#if UNITY_EDITOR
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

        private void RemoveItemEffect(ItemEffect remove)
        {
            ClearNullItemEffects();
            if (remove == null) return;
            _effects.Remove(remove);
            UnityEditor.AssetDatabase.RemoveObjectFromAsset(remove);
        }
    }
}
#endif

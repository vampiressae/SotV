using UnityEngine;
using Items;

public static class ItemUIUtils
{
    public static void HandleExtendedUI(this IHasAction item, ItemUI ui, ItemInfoWithActionsUI prefab)
    {
        var old = ui.GetComponentInChildren<ItemInfoWithActionsUI>();
        if (old)
        {
            old.Uninit();
            Object.Destroy(old.gameObject);
        }

        if (prefab == null)
        {
            Debug.LogError("No prefab was set for Actions UI!", ui);
            return;
        }

        var extended = Object.Instantiate(prefab, ui.transform);
        extended.name = "Extended UI";
        extended.Init(item, ui);
    }
}

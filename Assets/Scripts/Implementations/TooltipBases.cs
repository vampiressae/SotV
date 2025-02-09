using UnityEngine;
using TMPro;
//using Inventories;

public abstract class TooltipWithName<TComponent> : TooltipForComponent<TComponent>
{
    [SerializeField] private TMP_Text _name;

    public override void Init(Component target)
    {
        base.Init(target);
        _name.text = target.name;
    }
}

public abstract class TooltipWithInventory<TComponent> : TooltipWithName<TComponent>
{
    //[SerializeField] private InventoryUI _inventoryUI;

    public override void Init(Component target)
    {
        base.Init(target);
        //if (target is IHasInventory invnentory)
        //    _inventoryUI.Init(invnentory.Inventory);
    }
}

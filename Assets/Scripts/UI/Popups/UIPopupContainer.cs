using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Items;
using Inventory;

public class UIPopupContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private InventoryHolder _container;
    [SerializeField] private InventoryHolder _inventory;
    [Space]
    [SerializeField] private Button _buttonTakeAll;

    private void OnEnable() => _buttonTakeAll.onClick.AddListener(TakeAll);
    private void OnDisable() => _buttonTakeAll.onClick.RemoveListener(TakeAll);

    public void Init(string name, List<ItemData> items)
    {
        _title.text = name;
        _container.Init(items);
    }

    private void TakeAll() => _inventory.Actor.Info.Inventory.Add(_container.Items, true);
}

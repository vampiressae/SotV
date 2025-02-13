using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Items;
using Inventory;

public class UIPopupContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private InventoryHolder _container;

    public void Init(string name, List<ItemData> items)
    {
        _title.text = name;
        _container.Init(items);
    }
}

using Actor;
using System;

namespace Items
{
    [Serializable]
    public class ItemData : Item<ItemInfo>, IItemCanAdd
    {
        public event Action<ItemData> OnItemDataChanging;
        public event Action<ItemData> OnItemDataChanged;

        public override bool IsEmpty => Info == null || Amount < 1;
        public override bool IsFull => Info && Amount >= Info.Stack;

        public ItemData(ItemData data)
        {
            _amount = data._amount;
            Info = data.Info;
        }

        public int AddAmount(int add)
        {
            var remain = 0;
            var desired = _amount + add;

            if (add == 0) return add;
            if (add > 0 && desired > Info.Stack)
            {
                _amount = Info.Stack;
                remain = desired - Info.Stack;
            }
            else if (add < 0 && desired < 0)
            {
                _amount = 0;
                remain = _amount - desired;
            }
            else _amount = desired;

            InvokeOnChanged();
            return remain;
        }

        public override void InvokeOnItemDataChanging() => OnItemDataChanging?.Invoke(this);
        public override void InvokeOnItemDataChanged() => OnItemDataChanged?.Invoke(this);

        public override void Swap(Item with)
        {
            InvokeOnItemDataChanging();
            with.InvokeOnItemDataChanging();

            var temp = new ItemData(this);
            Copy(with);
            with.Copy(temp);

            InvokeOnItemDataChanged();
            with.InvokeOnItemDataChanged();
        }

        public override string ToString() => $"{(IsEmpty ? "EMPTY" : $"{Info.name} x {Amount}" + (IsFull ? "(Full)" : ""))}";
    }
}

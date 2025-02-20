using System;
using System.Linq;
using System.Collections.Generic;

namespace Items
{
    public static class ItemUtils
    {
        public static void Add(this List<ItemData> list, List<ItemData> adds, Func<ItemData, bool> filter = null)
            => list.Add(adds, false, filter);

        public static void Add(this List<ItemData> list, List<ItemData> adds, bool move, Func<ItemData, bool> filter = null)
        {
            foreach (var add in adds)
            {
                if (filter != null && !filter(add)) continue;

                if (add.IsEmpty)
                {
                    list.Add(new());
                    continue;
                }

                if (add.IsNotEmptyAndNotStackable)
                {
                    list.Add(add);
                    if (move) add.Empty();
                    continue;
                }

                var old = list.Where(data => data.Info == add.Info).FirstOrDefault();
                if (old != null && old.IsNotEmptyAndStackable)
                {
                    old.AddAmount(add.Amount);
                    if (move) add.Empty();
                    continue;
                }

                old = list.Where(data => data.IsEmpty).FirstOrDefault();
                if (old != null)
                {
                    old.Copy(add);
                    if (move) add.Empty();
                }
                else list.Add(add);
            }
        }
    }
}

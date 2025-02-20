using System.Collections.Generic;
using System.Linq;

namespace Items
{
    public static class ItemUtils
    {
        public static void Add(this List<ItemData> list, IEnumerable<ItemData> adds)
        {
            foreach (var add in adds)
            {
                if (add.IsEmpty)
                {
                    list.Add(new());
                    continue;
                }

                if (add.IsNotEmptyAndNotStackable)
                {
                    list.Add(add);
                    continue;
                }

                var old = list.Where(data => data.Info == add.Info).FirstOrDefault();
                if (old != null && old.IsNotEmptyAndStackable)
                {
                    old.AddAmount(add.Amount);
                    continue;
                }

                old = list.Where(data => data.IsEmpty).FirstOrDefault();
                if (old != null) old.Copy(add);
                else list.Add(add);
            }
        }
    }
}

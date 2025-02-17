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
                if (add.Info.Stack == 1)
                    list.Add(add);
                else
                {
                    var old = list.Where(item => item.Info == add.Info).FirstOrDefault();
                    if (old == null) list.Add(add);
                    else old.AddAmount(add.Amount);
                }
            }
        }
    }
}

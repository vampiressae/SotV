using Entity;
using Items;

public abstract class TransformStatInfo : ItemActionInfo
{
    public abstract bool Transfer(EntityHolder target, int value);
}

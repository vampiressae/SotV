public enum CompareInt { None, Equal, NotEqual, Smaller, SmallerOrEqual, Larger, LargerOrEqual }
public static class CompareUtils
{
    public static bool Compare(this CompareInt comparer, int a, int b)
    {
        switch (comparer)
        {
            case CompareInt.Equal: return a == b;
            case CompareInt.NotEqual: return a != b;
            case CompareInt.Smaller: return a < b;
            case CompareInt.SmallerOrEqual: return a <= b;
            case CompareInt.Larger: return a > b;
            case CompareInt.LargerOrEqual: return a >= b;
            default: return false;
        }
    }
}
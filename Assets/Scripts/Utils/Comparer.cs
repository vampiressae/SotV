public enum CompareInt { None, Equal, NotEqual, Smaller, SmallerOrEqual, Larger, LargerOrEqual }
public static class CompareUtils
{
    public static bool Compare(this CompareInt comparer, int a, int b) => comparer switch
    {
        CompareInt.Equal => a == b,
        CompareInt.NotEqual => a != b,
        CompareInt.Smaller => a < b,
        CompareInt.SmallerOrEqual => a <= b,
        CompareInt.Larger => a > b,
        CompareInt.LargerOrEqual => a >= b,
        _ => false,
    };

    public static string ToString(this CompareInt comparer, int a, int b) => comparer switch
    {
        CompareInt.Equal => $"{a} == {b} : {a == b}",
        CompareInt.NotEqual => $"{a} != {b} : {a != b}",
        CompareInt.Smaller => $"{a} < {b} : {a < b}",
        CompareInt.SmallerOrEqual => $"{a} <= {b} : {a <= b}",
        CompareInt.Larger => $"{a} > {b} : {a > b}",
        CompareInt.LargerOrEqual => $"{a} >= {b} : {a >= b}",
        CompareInt.None => $"Skipping comparison of {a} and {b}",
        _ => $"Undefined Comparer <b>{comparer}</b>",
    };
}
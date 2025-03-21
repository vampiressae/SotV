using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public struct IntRange
{
    [HorizontalGroup(50), OnValueChanged("OnValueChangedInInspector"), HideLabel] public int Min;
    [HorizontalGroup(62), OnValueChanged("OnValueChangedInInspector"), LabelWidth(10), LabelText("-")] public int Max;

    public IntRange(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public int RandomUnity => GetUnityRandom();

    public int GetUnityRandom(bool inlcusiveMax = true)
        => UnityEngine.Random.Range(Min, Max + (inlcusiveMax ? 1 : 0));

    public static bool operator ==(IntRange what, IntRange with) 
        => what.Min == with.Min && what.Max == with.Max;

    public static bool operator !=(IntRange what, IntRange with)
        => what.Min != with.Min || what.Max != with.Max;

    public static IntRange operator +(IntRange what, int with)
    { what.Min += with; what.Max += with; return what; }

    public static IntRange operator +(IntRange what, IntRange with)
    { what.Min += with.Min; what.Max += with.Max; return what; }

    public static IntRange operator -(IntRange what, int with)
    { what.Min -= with; what.Max -= with; return what; }

    public static IntRange operator -(IntRange what, IntRange with)
    { what.Min -= with.Min; what.Max -= with.Max; return what; }

    public static IntRange operator *(IntRange what, int with)
    { what.Min *= with; what.Max *= with; return what; }

    public static IntRange operator *(IntRange what, float with)
    {
        what.Min = Mathf.RoundToInt(what.Min * with);
        what.Max = Mathf.RoundToInt(what.Max * with);
        return what;
    }

    public override bool Equals(object obj) => obj is IntRange range && Min == range.Min && Max == range.Max;
    public override int GetHashCode() => HashCode.Combine(Min, Max);
    public override string ToString() => $"{Min}-{Max}";

    public static IntRange Zero => new IntRange(0, 0);
    public static IntRange One => new IntRange(1, 1);

#if UNITY_EDITOR
    private void OnValueChangedInInspector() { if (Min > Max) Max = Min; }
#endif
}

using Sirenix.OdinInspector;

[System.Serializable]
public struct IntRange
{
    [HorizontalGroup(50), HideLabel] public int Min;
    [HorizontalGroup(62), HideLabel, LabelWidth(10), LabelText("-")] public int Max;

    public IntRange(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public int RandomUnity => GetUnityRandom();

    public int GetUnityRandom(bool inlcusiveMax = true) 
        => UnityEngine.Random.Range(Min, Max + (inlcusiveMax ? 1 : 0));

    public override string ToString() => $"{Min}-{Max}";
}

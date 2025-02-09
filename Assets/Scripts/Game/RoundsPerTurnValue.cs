using Actor;
using Entity;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Values/Rounds Per Turn")]
public class RoundsPerTurnValue : ScriptableInt
{
    [SerializeField] private float _multiplier = 2;
    public int GetMightValue(EntityHolder actor, float might)
    {
        for (int i = 0; i < Value; i++)
            might = might * _multiplier;
        return Mathf.RoundToInt(might);
    }
}

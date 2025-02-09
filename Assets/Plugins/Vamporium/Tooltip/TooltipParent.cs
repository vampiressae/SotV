using UnityEngine;

public class TooltipParent : MonoBehaviour
{
   [SerializeField] private TooltipTrigger _trigger;

    private void Awake() => _trigger.SetParent(transform);
}

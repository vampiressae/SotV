using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private ScriptableEvent _eventMouseUp;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            _eventMouseUp.Trigger();
    }
}

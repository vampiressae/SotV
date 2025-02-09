using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;

    private Transform _transform;

    private void Start() => _transform = transform;
    private void Update() => _transform.eulerAngles = _rotation;
}

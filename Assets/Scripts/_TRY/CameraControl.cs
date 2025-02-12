using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform _t;
    private Camera _camera;

    private void Start()
    {
        _t = transform;
        _camera = _t.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        _camera.transform.LookAt(_t.position);
    }
}

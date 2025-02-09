using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector2 _speed;

    private bool _pressed;
    private Vector3 _startPosition;

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (!_pressed)
            {
                _startPosition = Input.mousePosition;
                _pressed = true;
            }

            var position = transform.position;
            var delta = Input.mousePosition - _startPosition;

            _startPosition = Input.mousePosition;
            position += new Vector3(delta.x * _speed.x, delta.z * _speed.y);
            transform.position = position;
        }
        else if (_pressed) _pressed = false;
    }
}

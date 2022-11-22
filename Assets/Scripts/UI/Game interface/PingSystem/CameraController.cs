using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Vector2 _pointVector;
    private const float Zero = 0f;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnLook(InputValue inputValue)
    {
        _pointVector = inputValue.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(Zero, _pointVector.x));
    }
}
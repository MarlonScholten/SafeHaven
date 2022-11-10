using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Vector2 pointVector;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void OnLook(InputValue inputValue)
    {
        pointVector = inputValue.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0f, pointVector.x));
    }
}
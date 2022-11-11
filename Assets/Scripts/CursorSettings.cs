using UnityEngine;

public class CursorSettings : MonoBehaviour
{
    [SerializeField] private bool hideCursor = true;

    private void Awake()
    {
        Cursor.visible = hideCursor;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

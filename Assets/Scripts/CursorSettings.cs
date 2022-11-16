using UnityEngine;

public class CursorSettings : MonoBehaviour
{
    /// <summary>
    /// Customize settings related to the cursor.
    /// </summary>
    [SerializeField] private bool _hideCursor = true;

    private void Awake()
    {
        Cursor.visible = _hideCursor;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

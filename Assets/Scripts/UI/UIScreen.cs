using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIScreen : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    private float _scale;
    private List<Canvas> _canvases;

    protected virtual void Awake()
    {
        _canvases = new();
    }

    private void ToggleCamera(bool state)
    {
        // Disable the camera.
        CinemachineBrain cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        if (cinemachineBrain)
            cinemachineBrain.enabled = state;
    }

    protected void Pause()
    {
        // Set the time.
        _scale = Time.timeScale;
        Time.timeScale = 0;

        // Set the cursor.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        // Disable the input.
        InputBehaviour.Instance.gameObject.SetActive(false);

        // Disable the camera.
        ToggleCamera(false);

        // Disable every enabled canvas.
        foreach (Canvas canvas in _canvases = FindObjectsOfType<Canvas>().Where(x => x.enabled && x != _canvas).ToList())
            canvas.enabled = false;
    }

    protected void Resume()
    {
        // Unpause.
        ToggleCamera(true);
        Time.timeScale = _scale;
        Cursor.visible = false;
        InputBehaviour.Instance.gameObject.SetActive(true);
        foreach (Canvas canvas in _canvases)
            canvas.enabled = true;
    }
}

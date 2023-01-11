using Cinemachine;
using SoundManager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScreen : MonoBehaviour
{
    [Header("Inheritance")]
    [SerializeField]
    [Tooltip("The canvas that'll be excluded when hiding other canvasses.")]
    private Canvas _canvas;

    [SerializeField]
    [Tooltip("The prefab that contains the UI for the loading screen.")]
    protected GameObject _loadingScreen;

    [SerializeField]
    [Tooltip("The id from the scene to load when the main menu button is pressed. The scene needs to be added to the build settings, the id is the place of the scene in that list.")]
    protected int _mainMenuSceneId;

    // Private.
    private float _scale;
    private List<Canvas> _canvases;

    // Protected.
    protected EnemyStateWatcher _enemyStateWatcher;

    protected virtual void Awake()
    {
        _canvases = new();
        _enemyStateWatcher = FindObjectOfType<EnemyStateWatcher>();
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

    protected void Restart()
    {
        _enemyStateWatcher?.StopSound();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    protected void Return()
    {
        Instantiate(_loadingScreen);
        _enemyStateWatcher?.StopSound();
        SceneManager.LoadSceneAsync(_mainMenuSceneId, LoadSceneMode.Single);
    }
}

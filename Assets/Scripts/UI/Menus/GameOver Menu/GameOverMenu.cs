using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    [Tooltip("The retry, 'start again' button. Should restart the level.")]
    private Button _retry;

    [SerializeField]
    [Tooltip("The return, 'return to menu' button. Should return you to the menu.")]
    private Button _return;

    [Header("Duplicate")]
    [SerializeField]
    [Tooltip("The prefab that contains the UI for the loading screen.")]
    private GameObject _loadingScreen;

    [SerializeField]
    [Tooltip("The id from the scene to load when the main menu button is pressed. The scene needs to be added to the build settings, the id is the place of the scene in that list.")]
    private int _mainMenuSceneId;

    private void Start()
    {
        _retry.onClick.AddListener(OnRetry);
        _return.onClick.AddListener(OnReturn);

        _retry.Select();
    }

    private void OnRetry()
    {

    }

    private void OnReturn()
    {
        Instantiate(_loadingScreen);
        GameObject.Find("EnemyStateWatcher").GetComponent<SoundManager.EnemyStateWatcher>().StopSound();
        SceneManager.LoadSceneAsync(_mainMenuSceneId, LoadSceneMode.Single);
    }
}

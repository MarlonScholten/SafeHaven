using Cinemachine;
using SoundManager;
using System.Linq;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The game over message if you get caught.")]
    [SerializeField]
    private string _sisterCaught = "Game over, you got caught!";

    [Tooltip("The game over message if your brother gets caught.")]
    [SerializeField]
    private string _brotherCaught = "Game over, your brother got caught!";

    [Header("References")]
    [Tooltip("The game over menu's prefab that'll be displayed upon game over.")]
    [SerializeField]
    private GameObject _gameOverMenu;

    private EnemyStateWatcher _enemyStateWatcher;

    private void Awake()
    {
        _enemyStateWatcher = FindObjectOfType<EnemyStateWatcher>();
        _enemyStateWatcher.OnSisterCaught += GameOverSister;
        _enemyStateWatcher.OnBrotherCaught += GameOverBrother;
    }

    private void GameOverSister() => GameOverInstantiate(_sisterCaught);

    private void GameOverBrother() => GameOverInstantiate(_brotherCaught);

    private void GameOverInstantiate(string message)
    {
        // Disallow multiple instances.
        _enemyStateWatcher.OnSisterCaught -= GameOverSister;
        _enemyStateWatcher.OnBrotherCaught -= GameOverBrother;

        // Setup prerequisites for an interactable menu. 
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        InputBehaviour.Instance.gameObject.SetActive(false);

        // Disable the camera.
        CinemachineBrain cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        if (cinemachineBrain)
            cinemachineBrain.enabled = false;

        // Disable every enabled canvas.
        foreach (Canvas canvas in FindObjectsOfType<Canvas>().Where(x => x.enabled))
            canvas.enabled = false;

        // Create the game over menu & set the message.
        GameOverMenu actual = Instantiate(_gameOverMenu).GetComponent<GameOverMenu>();
        actual.GameOverMessage = message;
    }
}

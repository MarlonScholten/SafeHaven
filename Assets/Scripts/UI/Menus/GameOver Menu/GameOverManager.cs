using Cinemachine;
using SoundManager;
using System.Linq;
using UnityEngine;

/// <summary>
/// Author: Hugo Verweij <br/>
/// Modified by:  <br/>
/// Description: <see cref="GameOverManager"/> handles the state and creation of the <see cref="GameOverMenu"/>.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>GameOverManager</term>
///         <term>Prefab</term>
///         <term>GameOverManager.cs</term>
///         <term>The prefab contains all the information needed to handle the state of the <see cref="GameOverMenu"/>.</term>
///     </item>
/// </list>
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

        // Create the game over menu & set the message.
        GameOverMenu actual = Instantiate(_gameOverMenu).GetComponent<GameOverMenu>();
        actual.GameOverMessage = message;
    }
}

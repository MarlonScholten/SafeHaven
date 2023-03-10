using SoundManager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// Author: Hugo Verweij <br/>
/// Modified by:  <br/>
/// Description: Handles the creation and state of the GameOverMenu.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>GameOverMenu</term>
///         <term>Prefab</term>
///         <term>GameOverMenu.cs</term>
///         <term>The prefab contains all the information needed to handle the state of the <see cref="GameOverMenu"/>.</term>
///     </item>
/// </list>
public class GameOverMenu : UIScreen
{
    /// <summary>
    /// Sets and returns the corresponding <see cref="TMP_Text"/> element, which displays the game over message.
    /// </summary>
    public string GameOverMessage
    {
        get => _message.text;
        set => _message.text = value;
    }

    [Header("References")]
    [SerializeField]
    [Tooltip("The game over message, will change depending on the state of the game.")]
    private TMP_Text _message;

    [SerializeField]
    [Tooltip("The retry, 'start again' button. Should restart the level.")]
    private Button _retry;

    [SerializeField]
    [Tooltip("The return, 'return to menu' button. Should return you to the menu.")]
    private Button _return;

    private void Start()
    {
        Pause();

        _retry.onClick.AddListener(OnRetry);
        _return.onClick.AddListener(OnReturn);

        _retry.Select();
    }

    private void OnRetry() => Restart();

    private void OnReturn() => Return();
}

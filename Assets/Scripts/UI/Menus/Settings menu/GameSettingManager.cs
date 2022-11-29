using UnityEngine;

/// <summary>
/// Author: Tom Cornelissen <br/>
/// Modified by:  <br/>
/// Description: This class handles taking the user settings and applying it to the game itself
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>GameSettingManager</term>
///         <term>Prefab</term>
///         <term>GameSettingManager.cs</term>
///         <term>This prefab contains the scripts that handle applying game settings</term>
///     </item>
/// </list>
public class GameSettingManager : MonoBehaviour
{
    private void Start()
    {
        ApplySettings();
    }

    /// <summary>
    /// Method <c>ApplySettings</c> applies all user settings to the game.
    /// </summary>
    public void ApplySettings()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
    }
}

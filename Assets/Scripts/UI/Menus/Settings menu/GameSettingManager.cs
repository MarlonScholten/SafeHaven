using Cinemachine;
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
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 0.75f);
        
        float sensitivity = Mathf.Max(0.1f , PlayerPrefs.GetFloat("Sensitivity", 1.0f) * 4.0f);
        
        var cameras = FindObjectsOfType<CinemachineFreeLook>();
        foreach (CinemachineFreeLook cinemachineFreeLook in cameras)
        {
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 2f * sensitivity;
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0.02f * sensitivity;
        }
    }
}

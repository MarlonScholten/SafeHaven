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
    [SerializeField]
    [Tooltip("A reference to the Wwise RTPC for the music volume")]
    private AK.Wwise.RTPC _musicVolume;
    
    [SerializeField]
    [Tooltip("A reference to the Wwise RTPC for the SFX volume")]
    private AK.Wwise.RTPC _sfxVolume;
    
    [SerializeField]
    [Tooltip("A reference to the Wwise RTPC for the footsteps volume")]
    private AK.Wwise.RTPC _footstepsVolume;
    
    [SerializeField]
    [Tooltip("A reference to the Wwise RTPC for the voicelines volume")]
    private AK.Wwise.RTPC _voicelinesVolume;

    [SerializeField]
    [Tooltip("Show debug logs for audio settings")]
    private bool _debugAudioSettings = true;
    
    private void Start()
    {
        ApplySettings();
    }

    /// <summary>
    /// Method <c>ApplySettings</c> applies all user settings to the game.
    /// </summary>
    public void ApplySettings()
    {
        float sensitivity = Mathf.Max(0.1f , PlayerPrefs.GetFloat("Sensitivity", 1.0f) * 4.0f);
        
        var cameras = FindObjectsOfType<CinemachineFreeLook>();
        foreach (CinemachineFreeLook cinemachineFreeLook in cameras)
        {
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 2f * sensitivity;
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0.02f * sensitivity;
        }
        
        ApplyAudioSettings();
    }

    private void ApplyAudioSettings()
    {
        if (_debugAudioSettings)
        {
            Debug.Log("Music volume set to " + PlayerPrefs.GetFloat("Music", 1.0f) * 100.0f);
            Debug.Log("SFX volume set to " + PlayerPrefs.GetFloat("SFX", 1.0f) * 100.0f);
            Debug.Log("Footsteps volume set to " + PlayerPrefs.GetFloat("Footsteps", 1.0f) * 100.0f);
            Debug.Log("Voicelines volume set to " + PlayerPrefs.GetFloat("Voicelines", 1.0f) * 100.0f);
        }
        
        _musicVolume.SetGlobalValue(PlayerPrefs.GetFloat("Music", 1.0f) * 100.0f);
        _sfxVolume.SetGlobalValue(PlayerPrefs.GetFloat("SFX", 1.0f) * 100.0f);
        _footstepsVolume.SetGlobalValue(PlayerPrefs.GetFloat("Footsteps", 1.0f) * 100.0f);
        _voicelinesVolume.SetGlobalValue(PlayerPrefs.GetFloat("Voicelines", 1.0f) * 100.0f);
    }
}

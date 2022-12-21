using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Tom Cornelissen <br/>
/// Modified by:  <br/>
/// Description: This class handles the logic of rebinding keys
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>SettingsMenu</term>
///         <term>Prefab</term>
///         <term>SettingsSlider</term>
///         <term>This prefab contains the scripts and UI elements that handle changing user settings</term>
///     </item>
///     <item>
///         <term>GameSettingManager</term>
///         <term>Prefab</term>
///         <term>GameSettingManager</term>
///         <term>This prefab contains the scripts that handle applying game settings</term>
///     </item>
///     <item>
///         <term>Slider</term>
///         <term>GameObject</term>
///         <term>Slider</term>
///         <term>The slider that handles the user input</term>
///     </item>
/// </list>
public class SettingsSlider : MonoBehaviour
{
    [SerializeField]
    [Tooltip("A reference to the slider UI element")]
    private Slider _slider;

    [SerializeField]
    [Tooltip("A reference to the text UI element")]
    private TMP_Text _text;

    [SerializeField]
    [Tooltip("The name of the setting to be changed")]
    private string _settingName;
    
    [SerializeField]
    [Tooltip("The default value of the setting to be changed")]
    private float _defaultValue = 1.0f;
    
    private void Start()
    {
        _text.text = _settingName;
        _slider.value = PlayerPrefs.GetFloat(_settingName, _defaultValue);
        
        _slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        PlayerPrefs.SetFloat(_settingName, _slider.value);
        PlayerPrefs.Save();
        
        FindObjectOfType<GameSettingManager>().ApplySettings();
    }
}

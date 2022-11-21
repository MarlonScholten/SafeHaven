using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSlider : MonoBehaviour
{
    [SerializeField] 
    private Slider _slider;

    [SerializeField] 
    private TMP_Text _text;

    [SerializeField] 
    private string _settingName;
    
    void Start()
    {
        _text.text = _settingName;
        _slider.value = PlayerPrefs.GetFloat(_settingName);
    }

    public void OnValueChanged()
    {
        PlayerPrefs.SetFloat(_settingName, _slider.value);
        PlayerPrefs.Save();
        
        FindObjectOfType<GameSettingManager>().ApplySettings();
    }
}

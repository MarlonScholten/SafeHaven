using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ApplySettings();
    }

    public void ApplySettings()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
    }
}

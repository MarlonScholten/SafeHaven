using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMusicOnStartup : MonoBehaviour
{
    private enum SoundScenes
    {
        Menu = 3,
        Main = 4
    };

    [SerializeField]
    private SoundScenes LoadSoundOnStart = SoundScenes.Main;
    void Start()
    {
        SceneManager.LoadScene((int)LoadSoundOnStart, LoadSceneMode.Additive);
    }
}

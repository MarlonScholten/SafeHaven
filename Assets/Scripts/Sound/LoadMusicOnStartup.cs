using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoundManager
{
    public class LoadMusicOnStartup : MonoBehaviour
    {
        private enum SoundScenes
        {
            MenuSound,
            MainSound
        };

        [SerializeField]
        private SoundScenes LoadSoundOnStart = SoundScenes.MainSound;
        void Start()
        {
            SceneManager.LoadSceneAsync(LoadSoundOnStart.ToString(), LoadSceneMode.Additive);
        }

        private void OnDestroy()
        {
            SceneManager.UnloadSceneAsync(LoadSoundOnStart.ToString());
        }
    }
}
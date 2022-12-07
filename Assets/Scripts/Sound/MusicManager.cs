using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager
{
    public class MusicManager : SoundBase
    {
        private void Start()
        {
            _gameObject = gameObject;
            playSound();
        }

        private void OnDestroy()
        {
            stopSound();
        }
    }
}

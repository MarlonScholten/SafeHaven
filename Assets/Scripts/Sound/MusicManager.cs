using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager
{
    public class MusicManager : SoundBase
    {
        private void Start()
        {
            playSound();
        }

        private void OnDestroy()
        {
            stopSound();
        }
    }
}

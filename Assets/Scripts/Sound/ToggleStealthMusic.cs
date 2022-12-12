using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager
{
    public class ToggleStealthMusic : SoundBase
    {
        void Start()
        {
            _stealthTrigger.OnDangerEnter += StartStealthMusic;
            _stealthTrigger.OnDangerExit += StopStealthMusic;

        }

        private void StartStealthMusic()
        {
            playSound(gameObject);
        }

        private void StopStealthMusic()
        {
            stopSound();
        }
        private void StartInvestegating()
        {

        }

        private void StartChasing()
        {

        }
    }
}

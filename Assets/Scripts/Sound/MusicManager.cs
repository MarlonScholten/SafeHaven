using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager
{
    public class MusicManager : SoundBase
    {
        public int test = 1;
        private void Start()
        {
            _gameObject = gameObject;
            StartCoroutine(cororo());
        }

        IEnumerator cororo()
        {
            yield return new WaitForSeconds(1);
            playSound();
        }

        private void OnDestroy()
        {
            stopSound();
        }
    }
}

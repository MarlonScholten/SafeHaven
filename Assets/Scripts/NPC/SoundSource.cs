using UnityEngine;

namespace NPC
{
    public class SoundSource
    {
        private readonly GameObject _source;
        private readonly float _volume;
        
        public SoundSource(GameObject source, float volume)
        {
            _source = source;
            _volume = volume;
        }
        
        public GameObject GetSource()
        {
            return _source;
        }
        
        public float GetVolume()
        {
            return _volume;
        }
    }
}
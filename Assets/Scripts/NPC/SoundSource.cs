using UnityEngine;

namespace NPC
{
    public class SoundSource
    {
        public GameObject source;
        public float volume;
        
        public SoundSource(GameObject source, float volume)
        {
            this.source = source;
            this.volume = volume;
        }
        
        public GameObject getSource()
        {
            return source;
        }
        
        public float getVolume()
        {
            return volume;
        }
    }
}
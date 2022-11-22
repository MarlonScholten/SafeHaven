using UnityEngine;

namespace NPC
{
    /// <summary>
    /// This script is an simulated sound source.
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///	    <item>
    ///         <term>MakeNoise</term>
    ///		    <term>Script</term>
    ///         <term>MakeNoise</term>
    ///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT Patrol states</term>
    ///	    </item>
    /// </list>
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
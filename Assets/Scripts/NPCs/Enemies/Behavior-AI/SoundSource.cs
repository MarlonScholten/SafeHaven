using UnityEngine;

namespace NPC
{
    /// <summary>
    /// Author: Hugo Ulfman<br/>
    /// Modified by: <br/>
    /// Description:  This script is an simulated sound source.
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
        private readonly GameObject _source; // The GameObject that is the source of the sound
        private readonly float _volume; // The volume of the sound
        
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
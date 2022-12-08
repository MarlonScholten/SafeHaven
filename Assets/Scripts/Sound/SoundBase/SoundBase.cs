using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SoundManager
{
    /// <summary>
    /// Author: Thomas van den Oever <br/>
    /// Modified by:  <br/>
    /// Description: a base class for a sound class
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///     <item>
    ///         <term>ON_WHAT</term>
    ///         <term>TYPE</term>
    ///         <term>NAME</term>
    ///         <term>DISCRIPTION</term>
    ///     </item>
    /// </list>

    [RequireComponent(typeof(AkGameObj))]
    public class SoundBase : MonoBehaviour
    {
        [SerializeField]
        public AK.Wwise.Event playEvent;

        [SerializeField]
        public AK.Wwise.Event stopEvent = null;

        [SerializeField]
        [Range(0, 100)]
        protected int _volume = 50;

        [SerializeField]
        [Range(0, 100)]
        protected int _attenuation = 50;

        protected GameObject _gameObject;

        protected virtual void playSound(GameObject soundGameObject = null)
        {
            if(soundGameObject != null)
            {
                _gameObject = soundGameObject;
            }

            playEvent.Post(_gameObject);
        }

        protected virtual void stopSound(int transitionTime = 0)
        {
            if (stopEvent.IsValid())
            {
                stopEvent.Post(_gameObject);
            }
            else
            {
                playEvent.Stop(_gameObject, transitionTime);
            }
        }
    }
}
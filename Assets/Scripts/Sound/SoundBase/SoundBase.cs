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

        private AK.Wwise.RTPC volumeRTPC;

        private AK.Wwise.RTPC attenuationRTPC;

        private bool _isPlaying = false;

        protected virtual void playSound(GameObject soundGameObject = null)
        {
            Debug.Log("Start Playing Sound");
            if(soundGameObject != null)
            {
                _gameObject = soundGameObject;
            }

            if (_isPlaying)
            {
                return;
            }
            playEvent.Post(_gameObject, (uint)AkCallbackType.AK_EndOfEvent, soundCallback);
            _isPlaying = true;
        }

        protected virtual void stopSound(int transitionTime = 0)
        {
            if (stopEvent.IsValid())
            {
                //Debug.Log("post the stop event");
                stopEvent.Post(_gameObject);
            }
            else
            {
                //Debug.Log("stop current event");
                playEvent.Stop(_gameObject, transitionTime);
            }
        }

        protected virtual int changeVolume(int volume)
        {
            if (volume > 0 || volume < 100)
            {
                _volume = volume;
            }
            volumeRTPC.SetGlobalValue(_volume);
            return _volume;
        }

        protected virtual int changeAttenuation(int attenuation)
        {
            if (attenuation > 0 || attenuation < 100)
            {
                _attenuation = attenuation;
            }
            attenuationRTPC.SetGlobalValue(_attenuation);
            return _attenuation;
        }

        protected virtual void soundCallback(object in_cookie, AkCallbackType in_type, object in_info)
        {
            //Debug.Log(in_type);
            if(in_type == AkCallbackType.AK_EndOfEvent)
            {
                _isPlaying = false;
            }
        }
    }
}
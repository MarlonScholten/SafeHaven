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

    [RequireComponent(typeof(AkGameObj))]
    public class SoundBase : MonoBehaviour
    {
        [SerializeField]
        public AK.Wwise.Event playEvent;

        [SerializeField]
        public AK.Wwise.Event stopEvent = null;

        [SerializeField]
        public List<AK.Wwise.Switch> switchEvent;

        [SerializeField]
        public List<AK.Wwise.State> states;

        [SerializeField]
        [Range(0, 100)]
        [Tooltip("Changes the volume")]
        protected List<float> _volume;

        [SerializeField]
        [Range(0, 100)]
        [Tooltip("Does nothing because of limitations of Wwise")]
        protected int _attenuation = 50;

        [SerializeField]
        protected StealthTrigger _stealthTrigger;

        protected GameObject _gameObject;


        protected virtual void playSound(GameObject soundGameObject = null)
        {
            if(soundGameObject != null)
            {
                _gameObject = soundGameObject;
            }

            if (playEvent.IsValid())
            {
                playEvent.Post(_gameObject);
            }
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
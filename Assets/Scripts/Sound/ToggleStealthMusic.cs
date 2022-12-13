using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager
{
    /// <summary>
    /// Author: Thomas van den Oever <br/>
    /// Modified by:  <br/>
    /// Description: Toggles the stealth music on and off. also changes the state of the music depending on the events that are called
    /// <br/>I am so sorry for this mess I had no time to make an actual state machine
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
    public class ToggleStealthMusic : SoundBase
    {
        private EnemyStateWatcher _watcher;
        private bool _isInvestigating = false;
        private bool _isChasing = false;

        protected enum MusicState
        {
            None = 0,
            Hidden = 1,
            Caution = 2,
            Alert = 3
        };

        void Start()
        {
            _stealthTrigger.OnDangerEnter += StartStealthMusic;
            _stealthTrigger.OnDangerExit += StopStealthMusic;

            _watcher = GameObject.Find("EnemyStateWatcher").GetComponent<EnemyStateWatcher>();
            _watcher.OnInvestegating += StartInvestegating;
            _watcher.OnZeroInvestegating += StopInvestegating;
            _watcher.OnChasing += StartChasing;
            _watcher.OnZeroChasing += StopChasing;
            _watcher.StopIt += StopMusic;
        }

        //TODO: refactor this mess into a state machine, I'm so sorry
        private void StartStealthMusic()
        {
            states[(int)MusicState.Hidden].SetValue();
            playSound(gameObject);
        }

        public void StopStealthMusic()
        {
            states[(int)MusicState.None].SetValue();
            stopSound();
        }
        private void StartInvestegating()
        {
            _isInvestigating = true;
            if (_isChasing)
            {
                return;
            }
            states[(int)MusicState.Caution].SetValue();

        }

        private void StopInvestegating()
        {
            _isInvestigating = false;
            if (_isChasing)
            {
                states[(int)MusicState.Alert].SetValue();
            }
            else
            {
                states[(int)MusicState.Hidden].SetValue();
            }
        }

        private void StartChasing()
        {
            _isChasing = true;
            states[(int)MusicState.Alert].SetValue();
        }

        private void StopChasing()
        {
            _isChasing = false;
            if (_isInvestigating)
            {
                states[(int)MusicState.Caution].SetValue();
            }
            else
            {
                states[(int)MusicState.Hidden].SetValue();
            }

        }

        private void OnDestroy()
        {
            Debug.LogWarning("DESTORY SOUND");
            stopSound();
        }

        private void StopMusic()
        {
            stopSound();
        }
    }
}

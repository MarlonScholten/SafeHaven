using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager
{
    /// <summary>
    /// Author: Thomas van den Oever <br/>
    /// Modified by:  <br/>
    /// Description: Helper function to kind of interface with the soundsystem 
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///     <item>
    ///         <term>Enemy prefabs</term>
    ///         <term>Prefab</term>
    ///         <term>EnemyObject</term>
    ///         <term>multiple scripts on that prefab use this scripts functions</term>
    ///     </item>
    /// </list>
    public class EnemyStateWatcher : MonoBehaviour
    {
        private int _isInvestegating = 0;
        private int _isChasing = 0;

        /// <summary>
        /// Event handler for the <see cref="EnemyStateWatcher"/>.
        /// </summary>
        public delegate void EnemyStateWachterEvent();
        public event EnemyStateWachterEvent OnInvestegating;
        public event EnemyStateWachterEvent OnChasing;
        public event EnemyStateWachterEvent OnZeroInvestegating;
        public event EnemyStateWachterEvent OnZeroChasing;
        public event EnemyStateWachterEvent StopIt;
        /// <summary>
        /// Sister caught event, fires when sister is caught.
        /// </summary>
        public event EnemyStateWachterEvent OnSisterCaught;
        /// <summary>
        /// Brother caught event, fires when brother is caught.
        /// </summary>
        public event EnemyStateWachterEvent OnBrotherCaught;

        /// <summary>
        /// Invoke the stop sound event
        /// </summary>
        public void StopSound()
        {
            StopIt?.Invoke();
        }

        /// <summary>
        /// Invokes the OnInvestegating and OnZeroInvestegating events based on if enemy's are investagating
        /// </summary>
        /// <param name="investegating">set true if enemy starts investegating, set false if enemy stops investegating</param>
        public void IsInvestegating(bool investegating)
        {
            if (investegating)
            {
                if(_isInvestegating == 0)
                {
                    OnInvestegating?.Invoke();
                }
                _isInvestegating++;
            }
            else
            {
                _isInvestegating--;
                if(_isInvestegating == 0)
                {
                    OnZeroInvestegating?.Invoke();
                }
            }
        }

        /// <summary>
        /// Invokes the OnChasing and OnZeroChasing events based on if there enemy's are chasing
        /// </summary>
        /// <param name="chasing">set true if enemy starts chasing, set false if enemy stops chasing</param>
        public void IsChasing(bool chasing)
        {
            if (chasing)
            {
                if(_isInvestegating == 0)
                {
                    OnChasing?.Invoke();
                }
                _isChasing++;
            }
            else
            {
                _isChasing--;
                if (_isChasing == 0)
                {
                    OnZeroChasing?.Invoke();
                }
            }
        }

        /// <summary>
        /// Relays the caught event to a centralized component like <see cref="EnemyStateWatcher"/>. See <see cref="EnemyAiStateManager.CatchChild(GameObject)"/> for documentation.
        /// </summary>
        public void SisterCaught() => OnSisterCaught?.Invoke();

        /// <summary>
        /// Relays the caught event to a centralized component like <see cref="EnemyStateWatcher"/>. See <see cref="EnemyAiStateManager.CatchChild(GameObject)"/> for documentation.
        /// </summary>
        public void BrotherCaught() => OnBrotherCaught?.Invoke();
    }
}
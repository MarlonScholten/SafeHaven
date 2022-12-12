using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager
{
    public class EnemyStateWatcher : MonoBehaviour
    {
        private int _isInvestegating = 0;
        private int _isChasing = 0;

        public delegate void EnemyStateWachterEvent();
        public event EnemyStateWachterEvent OnInvestegating;
        public event EnemyStateWachterEvent OnChasing;
        public event EnemyStateWachterEvent OnZeroInvestegating;
        public event EnemyStateWachterEvent OnZeroChasing;

        public void isInvestegating(bool investegating)
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

        public void isChasing(bool chasing)
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

    }
}
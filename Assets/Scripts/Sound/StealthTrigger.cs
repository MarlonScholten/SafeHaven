using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SoundManager
{
    [RequireComponent(typeof(BoxCollider))]
    public class StealthTrigger : MonoBehaviour
    {
        public delegate void DangerZoneEvent();
        public event DangerZoneEvent OnDangerEnter;
        public event DangerZoneEvent OnDangerExit;

        private GameObject _player;

        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _player)
            {
                Debug.LogWarning(other.gameObject);
                OnDangerEnter?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == _player)
            {
                OnDangerExit?.Invoke();
            }
        }
    }
}
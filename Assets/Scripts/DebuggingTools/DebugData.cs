using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DebuggingTools
{
    public class DebugDataChangeEvent : UnityEvent<List<GameObject>>
    {
    }
    
    public class DebugData : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _debugGameObjects;

        public DebugDataChangeEvent DebugDataChangedEvent;

        private void Awake()
        {
            DebugDataChangedEvent = new DebugDataChangeEvent();
        }

        public void AddDebugGameObject(GameObject gameObjectToAdd)
        {
            _debugGameObjects.Add(gameObjectToAdd);
            
            DebugDataChangedEvent.Invoke(_debugGameObjects);
        }

        public List<GameObject> GetDebugGameObjects()
        {
            return _debugGameObjects;
        }
    }
}

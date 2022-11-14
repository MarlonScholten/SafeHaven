using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DebuggingTools
{
    public class DebugDataChangeEvent : UnityEvent<List<GameObject>>
    {
    }
    
    public class DebugData : MonoBehaviour
    {
        [SerializeField] private List<GameObject> debugGameObjects;

        public DebugDataChangeEvent DebugDataChangedEvent;

        private void Awake()
        {
            DebugDataChangedEvent = new DebugDataChangeEvent();
        }

        public void AddDebugGameObject(GameObject gameObjectToAdd)
        {
            debugGameObjects.Add(gameObjectToAdd);
            
            DebugDataChangedEvent.Invoke(debugGameObjects);
        }

        public List<GameObject> GetDebugGameObjects()
        {
            return debugGameObjects;
        }
    }
}

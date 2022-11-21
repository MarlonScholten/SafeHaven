using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DebuggingTools
{
    /// <summary>
    /// Unity event that returns a <c>List</c> of <c>GameObject></c>s
    /// </summary>
    public class DebugDataChangeEvent : UnityEvent<List<GameObject>>
    {
    }
    
    /// <summary>
    /// Class <c>DebugData</c> holds data related to the Debugging UI for example the objects that are listed.
    /// </summary>
    public class DebugData : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _debugGameObjects;

        /// <summary>
        /// This <c>Unity Event</c> is called when the debug data changes
        /// </summary>
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

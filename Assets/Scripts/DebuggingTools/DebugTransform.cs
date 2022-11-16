using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DebuggingTools
{
    /// <summary>
    /// Class <c>DebugTransform</c> handles displaying the transform of a object in the debugging tool.
    /// </summary>
    public class DebugTransform : MonoBehaviour, IDebuggableObject
    {
        [Header("Debug properties")]
    
        [SerializeField] 
        private bool _debugPosition = true;
    
        [SerializeField] 
        private bool _debugRotation = true;
    
        [SerializeField] 
        private bool _debugScale = true;
        
        [Header("Other")]
    
        [SerializeField]
        private bool _autoActivate = true;
    
        private void Start()
        {
            if (!_autoActivate) return;
        
            FindObjectOfType<DebugData>().AddDebugGameObject(gameObject);
        }

        /// <summary>
        /// Sends debug values to the debugging tool
        /// </summary>
        /// <returns>A dictionary of all values to be displayed in the debugging tool</returns>
        public Dictionary<string, string> GetDebugValues()
        {
            if (!_debugPosition && !_debugRotation && !_debugScale) return new Dictionary<string, string>();

            Dictionary<string, string> debugDictionary = new Dictionary<string, string>();
        
            if (_debugPosition) debugDictionary.Add("Position", transform.position.ToString());
            if (_debugRotation) debugDictionary.Add("Rotation", transform.rotation.eulerAngles.ToString());
            if (_debugScale) debugDictionary.Add("Scale", transform.localScale.ToString());

            return debugDictionary;
        }
    }
}

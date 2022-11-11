using System.Collections.Generic;
using UnityEngine;

namespace DebuggingTools
{
    /// <summary>
    /// Class <c>DebugTransform</c> handles displaying the transform of a object in the debugging tool.
    /// </summary>
    public class DebugTransform : MonoBehaviour, IDebuggableObject
    {
        [Header("Debug properties")]
    
        [SerializeField] 
        private bool debugPosition = true;
    
        [SerializeField] 
        private bool debugRotation = true;
    
        [SerializeField] 
        private bool debugScale = true;
    
        [Header("Other")]
    
        [SerializeField]
        private bool autoActivate = true;
    
        private void Start()
        {
            if (!autoActivate) return;
        
            FindObjectOfType<DebugUI>().AddDebugGameObject(gameObject);
        }

        /// <summary>
        /// Sends debug values to the debugging tool
        /// </summary>
        /// <returns>A dictionary of all values to be displayed in the debugging tool</returns>
        public Dictionary<string, string> GetDebugValues()
        {
            if (!debugPosition && !debugRotation && !debugScale) return new Dictionary<string, string>();

            Dictionary<string, string> debugDictionary = new Dictionary<string, string>();
        
            if (debugPosition) debugDictionary.Add("Position", transform.position.ToString());
            if (debugRotation) debugDictionary.Add("Rotation", transform.rotation.eulerAngles.ToString());
            if (debugScale) debugDictionary.Add("Scale", transform.localScale.ToString());

            return debugDictionary;
        }
    }
}

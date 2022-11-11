using System.Collections.Generic;
using UnityEngine;

namespace DebuggingTools
{
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

using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace DebuggingTools
{
    public class TestTracking : MonoBehaviour, IDebuggableObject
    {
        [SerializeField]
        private float trackedVariable = 45.0f;

        public Dictionary<string, string> GetDebugValues()
        {
            return new Dictionary<string, string>
            {
                { "Tracked Variable", trackedVariable.ToString(CultureInfo.CurrentCulture) },
                { "Tracked Variable 2", "Yes"}
            };
        }
        
        private void Update()
        {
            trackedVariable = Random.Range(0.0f, 100.0f);
        }
    }
}

using TMPro;
using UnityEngine;

namespace DebuggingTools
{
    /// <summary>
    /// Class <c>DebugKeyValueUI</c> displays a variable name and a value next to each other
    /// </summary>
    public class DebugKeyValueUI : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI keyWidget;
    
        [SerializeField] 
        private TextMeshProUGUI valueWidget;

        /// <summary>
        /// This method sets the <c>variable name</c> that is displayed
        /// </summary>
        /// <param name="variableName">The new variable name</param>
        /// <returns>returns itself to allow method chaining</returns>
        public DebugKeyValueUI SetVariableName(string variableName)
        {
            keyWidget.SetText(variableName);
        
            return this;
        }

        /// <summary>
        /// This method sets the <c>variable value</c> that is displayed
        /// </summary>
        /// <param name="variableValue">The new variable value</param>
        /// <returns>returns itself to allow method chaining</returns>
        public DebugKeyValueUI SetVariableValue(string variableValue)
        {
            valueWidget.SetText(variableValue);

            return this;
        }
    }
}

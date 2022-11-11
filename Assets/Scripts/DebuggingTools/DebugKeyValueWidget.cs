using TMPro;
using UnityEngine;

namespace DebuggingTools
{
    public class DebugKeyValueWidget : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI keyWidget;
    
        [SerializeField] 
        private TextMeshProUGUI valueWidget;

        public DebugKeyValueWidget SetVariableName(string variableName)
        {
            keyWidget.SetText(variableName);
        
            return this;
        }

        public DebugKeyValueWidget SetVariableValue(string variableValue)
        {
            valueWidget.SetText(variableValue);

            return this;
        }
    }
}

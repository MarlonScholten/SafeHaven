using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DebuggingTools
{
    /// <summary>
    /// Class <c>DebugGameObjectUI</c> handles displaying information about a <c>GameObject</c> in the debug menu
    /// </summary>
    public class DebugGameObjectUI : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _debugKeyValueWidget;
    
        [SerializeField] 
        private GameObject _debugHeaderWidget;
    
        private GameObject _debugGameObject;
        private IDebuggableObject[] _debugGameObjectScripts;
        private Dictionary<string, DebugKeyValueUI> _debugWidgets = new();

        /// <summary>
        /// This method initializes the Game Object widget using a given <c>GameObject.</c>
        /// This method has to be called before this widget can display information.
        /// </summary>
        /// <param name="debugGameObject">The <c>GameObject</c> that this widget represents</param>
        public void Initialize(GameObject debugGameObject)
        {
            _debugGameObject = debugGameObject;
            _debugGameObjectScripts = _debugGameObject.GetComponents<IDebuggableObject>();

            CreateDebugHeader(_debugGameObject.name);
            StartCoroutine(UpdateDebugWidgetCoroutine());
        }

        private IEnumerator UpdateDebugWidgetCoroutine()
        {
            while (true)
            {
                UpdateDebugWidget();
                yield return new WaitForSeconds(0.05f);
            }
        }

        private void UpdateDebugWidget()
        {
            foreach (IDebuggableObject debugGameObjectScript in _debugGameObjectScripts)
            {
                foreach (var debugValue in debugGameObjectScript.GetDebugValues())
                {
                    SetDebugValue(debugValue.Key, debugValue.Value);
                }
            }
        }
    
        private void SetDebugValue(string variableName, string variableValue)
        {
            if (!_debugWidgets.ContainsKey(variableName))
                _debugWidgets.Add(variableName, Instantiate(_debugKeyValueWidget, transform).GetComponent<DebugKeyValueUI>());

            if (_debugWidgets.TryGetValue(variableName, out var debugWidget))
                debugWidget.SetVariableName(variableName).SetVariableValue(variableValue);
        }
    
        private void CreateDebugHeader(string headerName)
        {
            Instantiate(_debugHeaderWidget, transform)
                .GetComponent<DebugHeaderUI>()
                .SetHeaderText(headerName);
        }
    }
}

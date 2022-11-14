using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DebuggingTools
{
    /// <summary>
    /// Unity event that returns a <c>bool</c> that resembles if the debugging tool is enabled or disabled
    /// </summary>
    public class ToggleEvent : UnityEvent<bool>
    {
    }

    public class DebugToggle : MonoBehaviour
    {        
        /// <summary>
        /// To this event can be subscribed to receive a event when the debugging tool is enabled or disabled
        /// <returns>A boolean that resembles if the debugging tool is enabled or disabled</returns>
        /// </summary>
        public ToggleEvent ToggleDebuggingToolsEvent;

        private bool _debuggerEnabled;

        private void Awake()
        {
            ToggleDebuggingToolsEvent = new ToggleEvent();
            
            StartCoroutine(LateStart());
        }
        
        private IEnumerator LateStart()
        {
            yield return new WaitForFixedUpdate();
            
            ToggleDebuggingToolsEvent.Invoke(_debuggerEnabled);
        }

        private void OnShowDebuggingTools()
        {
            _debuggerEnabled = !_debuggerEnabled;
            ToggleDebuggingToolsEvent.Invoke(_debuggerEnabled);
        }
    }
}
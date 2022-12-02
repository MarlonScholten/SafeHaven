using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DebuggingTools
{
    /// <summary>
    /// Unity event that returns a <c>bool</c> that resembles if the debugging tool is enabled or disabled
    /// </summary>
    public class ToggleEvent : UnityEvent<bool>
    {
    }

    /// /// <summary>
    /// Author: Tom Cornelissen <br/>
    /// Modified by: Hugo Verweij <br/>
    /// DebugToggle behaviour. Handles what should happen when the debug menu is toggled. <br />
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///	    <item>
    ///         <term>DebuggingTools Prefab</term>
    ///		    <term>Script</term>
    ///         <term>DebugToggle</term>
    ///		    <term>DebugToggle behaviour. Handles what should happen when the debug menu is toggled.</term>
    ///	    </item>
    /// </list>
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

        private void Start()
        {
            InputBehaviour.Instance.OnToggleDebugginToolsEvent += OnToggleDebugginTools;
        }

        private IEnumerator LateStart()
        {
            yield return new WaitForFixedUpdate();
            
            ToggleDebuggingToolsEvent.Invoke(_debuggerEnabled);
        }

        public void OnToggleDebugginTools()
        {
            _debuggerEnabled = !_debuggerEnabled;
            ToggleDebuggingToolsEvent.Invoke(_debuggerEnabled);
        }
    }
}
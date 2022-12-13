using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

    /// <summary>
    /// Author: Marlon Kerstens<br/>
    /// Modified by: N/A<br/>
    /// The start state of the Bird AI state machine.
    /// It waits a frame so that everything can be instantiated.
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///     <item>
    ///         <term>BirdObject (Assets/Prefabs/NPCs/Enemies/Bird/BirdObject.prefab)</term>
    ///		    <term>Component</term>
    ///         <term>BirdObject</term>
    ///		    <term>This script contains the ENTER/(FIXED)UPDATE/EXIT methods for the Initialize states.</term>
    ///	    </item>
    ///	    <item>
    ///         <term>BirdObject</term>
    ///		    <term>Visual scripting</term>
    ///         <term>BirdVisualScripting (Assets/Scripts/NPCs/Enemies/Birds/StateMachine/visualScripting/BirdVisualScripting.asset)</term>
    ///		    <term>This script need to be added to the Bird with the BirdVisualScripting</term>
    ///	    </item>
    /// </list>
    public class InitializeStateBird : MonoBehaviour
    {
        /// <summary>
        /// The enter method for the initialize state, it waits a frame until it goes to the start state.
        /// </summary>
        public void InitializeEnter()
        {
            StartCoroutine(WaitForNextFrame());
        }

        /// <summary>
        /// The update method for the initialize state.
        /// </summary>
        public void InitializeUpdate()
        {
        }

        /// <summary>
        /// The Fixed update method for the initialize state.
        /// </summary>
        public void InitializeFixedUpdate()
        {
        }

        /// <summary>
        /// The exit method for the initialize state.
        /// </summary>
        public void InitializeExit()
        {
        }

        private IEnumerator WaitForNextFrame()
        {
            yield return new WaitForEndOfFrame();
            CustomEvent.Trigger(this.gameObject, "Walking");
        }
    }
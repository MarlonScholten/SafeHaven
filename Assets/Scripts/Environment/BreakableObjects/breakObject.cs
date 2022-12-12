using UnityEngine;
using UnityEngine.Events;

namespace BreakableObjects
{
    /// <summary>
    /// Author: Marlon Kerstens <br/>
    /// Modified by: none <br/>
    /// Description: This script can be used to destroy an object when it is above a certain force threshold. It also has an event that can be used to trigger other actions like an sound effect and animation.
    /// How To Use: Attach this script to the object you want to break. Set the force threshold and the event that should be triggered when the object is destroyed.
    /// </summary>
    /// 
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///	    <item>
    ///         <term>Any (eg. Bottle, Window)</term>
    ///		    <term>Script</term>
    ///         <term>BreakObject</term>
    ///		    <term>The UnityEvent can be used by adding the function of another into the objectIsBroken field in the inspector. To destroy the game object after the animation or sound effect the function DestroyObject can be used.</term>
    ///	    </item>
    /// </list>
    public class BreakObject : MonoBehaviour
    {
        [Tooltip("How much force is needed to break the object")] [SerializeField] private float thresholdToBreak = 20f; 
        [Tooltip("Event to listen to so that other scripts can be triggered when the object is broken")] [SerializeField] private UnityEvent objectIsBroken;

        /// <summary>
        /// This method is called when the object collides with another object.
        /// It checks if the force of the collision is greater than the threshold to break.
        /// It calls the objectIsBroken event if the object is broken so the animation and sounds can be played.
        /// </summary>
        /// <param name="col">The object that it collides with.</param>
        private void OnCollisionEnter(Collision col) {
            Debug.Log(col.relativeVelocity.magnitude);
            if(col.relativeVelocity.magnitude > thresholdToBreak) objectIsBroken?.Invoke();
        }
        
        /// <summary>
        /// This function can be called by an external UnityEvent to destroy the game object.
        /// </summary>
        public void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}

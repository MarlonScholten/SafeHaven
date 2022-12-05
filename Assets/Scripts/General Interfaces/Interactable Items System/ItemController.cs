using System;
using System.Collections.Generic;
using UnityEngine;

namespace InteractableItemsSystem
{
    /// <summary>
    /// Author: Jasper Driessen <br/>
    /// Modified by:  <br/>
    /// Description: This script is for controlling an Item. This should be added on each item with the
    /// corresponding scriptable object (Item).
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///     <item>
    ///         <term>Item GameObject</term>
    ///         <term>Script</term>
    ///         <term>ItemController</term>
    ///         <term>Used for controlling the items.</term>
    ///     </item>
    ///     <item>
    ///         <term>Item GameObject</term>
    ///         <term>SerializeField</term>
    ///         <term>ItemController - Item</term>
    ///         <term>Add the corresponding scriptable object (Item). Stores the data of an item.</term>
    ///     </item>
    ///     <item>
    ///         <term>Item GameObject</term>
    ///         <term>Component</term>
    ///         <term>Collider</term>
    ///         <term>This is needed so the ray of the player can interact with the Item.</term>
    ///     </item>
    ///   <item>
    ///         <term>Item GameObject</term>
    ///         <term>Layer</term>
    ///         <term>InteractableItem</term>
    ///         <term>Ray uses this layer to detect the Item.</term>
    ///     </item>
    ///     <item>
    ///         <term>Item GameObject</term>
    ///         <term>Component</term>
    ///         <term>Rigidbody</term>
    ///         <term>So the item uses the physics system.</term>
    ///     </item>
    ///      <item>
    ///         <term>Player GameObject</term>
    ///         <term>Script</term>
    ///         <term>Player Controller - Cam Raycast layer</term>
    ///         <term>Add InteractableItems layer to the Cam Raycast layer in Player Controller.
    ///         So the raycast is also casted on that layer.</term>
    ///     </item>
    /// </list>
    public class ItemController : MonoBehaviour
    {
        [Tooltip("Add scriptable object Item of the item you want.")][SerializeField]private ItemSO _item;

        /// <summary>
        /// Stores the data of an item.
        /// </summary>
        public ItemSO Item => _item;

        private Rigidbody _rb;
        private Collider _col;
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _col = GetComponent<Collider>();
        }

        /// <summary>
        /// Disables the collider and the gravity on the rigidbody. Activates the isKinematic on the rigidbody.
        /// </summary>
        public void PickUpItem()
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;

            _col.enabled = false;
        }
        
        /// <summary>
        /// Activates the collider and the gravity on the rigidbody. Disables the isKinematic on the rigidbody.
        /// </summary>
        public void DropItem()
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
            
            _col.enabled = true;
        }
    }
}


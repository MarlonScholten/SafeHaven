using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


namespace InteractableItemsSystem
{
    /// <summary>
    /// Author: Jasper Driessen <br/>
    /// Modified by:  <br/>
    /// Description: Controls an interactable object, that can interact with other items. For example a key to a door.
    /// You should add the item that is needed for interaction with the object to "Item needed to interact"
    /// and check if the name should be important or not with the checkbox "Name Important"
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///     <item>
    ///         <term>The GameObject of your interactable object</term>
    ///         <term>Script</term>
    ///         <term>InteractableObject</term>
    ///         <term>This scripts holds the data of what item is needed to interact with the object.</term>
    ///     </item>
    ///     <item>
    ///         <term>The GameObject of your interactable object</term>
    ///         <term>Component</term>
    ///         <term>Collider</term>
    ///         <term>This is needed so the ray of the player can interact with the object.</term>
    ///     </item>
    ///   <item>
    ///         <term>The GameObject of your interactable object</term>
    ///         <term>Layer</term>
    ///         <term>InteractableItem</term>
    ///         <term>Ray uses this layer to detect the object.</term>
    ///     </item>
    ///      <item>
    ///         <term>Player GameObject</term>
    ///         <term>Script</term>
    ///         <term>Player Controller - Cam Raycast layer</term>
    ///         <term>Add InteractableItems layer to the Cam Raycast layer in Player Controller.
    ///         So the raycast is also casted on that layer.</term>
    ///     </item>
    /// </list>
    public class InteractableObject : MonoBehaviour
    {
        [Tooltip("Item ScriptableObject that is needed for interaction with object.")][SerializeField] 
        private ItemSO _itemNeededToInteract;
        [Tooltip("Determines if the name should be important or not.")][SerializeField] private bool _nameImportant;

        [Tooltip("Event to listen to so that other scripts can be triggered when interacted with interactable object.")]
        [SerializeField]
        private UnityEvent _interactWithObjectEvent;

        [Tooltip(
            "Event to listen to so that other scripts can be triggered when interacted with interactable object is not possible. For example wrong item..")]
        [SerializeField]
        private UnityEvent _canNotInteractWithObjectNoItemEvent;
        [SerializeField]
        private UnityEvent _canNotInteractWithObjectWrongItemEvent;
        [SerializeField]
        private UnityEvent _canNotInteractWithObjectWrongNameEvent;

        public UnityEvent InteractWithObjectEvent => _interactWithObjectEvent;
        public UnityEvent CanNotInteractWithObjectNoItemEvent => _canNotInteractWithObjectNoItemEvent;
        public UnityEvent CanNotInteractWithObjectWrongItemEvent => _canNotInteractWithObjectWrongItemEvent;
        public UnityEvent CanNotInteractWithObjectWrongNameEvent => _canNotInteractWithObjectWrongNameEvent;
        
        /// <summary>
        /// Stores which item is needed for the player to interact with the object.
        /// </summary>
        public ItemSO ItemNeededToInteract => _itemNeededToInteract;
        /// <summary>
        /// Determines if the name should be important or not.
        /// </summary>
        public bool NameImportant => _nameImportant;

        private void Start()
        {
            if (_itemNeededToInteract == null)
            {
                Debug.LogError("Item Needed To Interact in " + this + " has not been assigned!");
            }
        }
        
        public static void InteractWithObject()
        {
            Debug.Log("Interacted");
        }
        
        public static void CanNotInteractNoItem()
        {
            Debug.Log("No Item");
        }
        
        public static void CanNotInteractWrongItem()
        {
            Debug.Log("Wrong Item");
        }
        
        public static void CanNotInteractWrongName()
        {
            Debug.Log("Wrong Name");
        }
    }
}


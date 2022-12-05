using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractableItemsSystem
{
    /// <summary>
    /// Author: Jasper Driessen <br/>
    /// Modified by:  <br/>
    /// Description: This script is used for making the player able to throw items.
    /// 1. Drag the "Player Interact System" prefab under the player GameObject.
    /// The ThrowableItemController should already be on this prefab.
    /// 2. Drag the main camera into the serialize field "Cam".
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///     <item>
    ///         <term>Player GameObject</term>
    ///         <term>Prefab</term>
    ///         <term>Player Interact System</term>
    ///         <term>Used for controlling the interaction with items of the player.</term>
    ///     </item>
    ///     <item>
    ///         <term>Player GameObject</term>
    ///         <term>Script</term>
    ///         <term>ThrowableItemController - Cam</term>
    ///         <term>Add the main camera to the serialize field of Cam.</term>
    ///     </item>
    /// </list>
    public class ThrowableItemController : MonoBehaviour
    {
        [Tooltip("The force that you throw an item forward.")][SerializeField] private float _throwForceForward;
        [Tooltip("The force that you throw an item upwards.")][SerializeField] private float _throwForceUpwards;
        [Tooltip("The main camera.")][SerializeField] private Camera _cam;
        
        private Inventory _inventory;
        private PlayerItemInteraction _playerItemInteraction;

        private void Start()
        {
            _inventory = GetComponent<Inventory>();
            _playerItemInteraction = GetComponent<PlayerItemInteraction>();
            InputBehaviour.Instance.OnThrowEvent += OnThrowItem;
        }

        private void OnThrowItem()
        {
            if (!_inventory.HasItemInInventory) return;

            if (!_inventory.ItemInInventory.IsThrowable)
            {
                Debug.Log("Can't Throw Item");
            }
            else StartCoroutine(ThrowItem());
        }

        private IEnumerator ThrowItem()
        {
            _playerItemInteraction.isThrowingItem = true;
            
            var itemInInventory = _inventory.ItemInInventoryObj;
            var itemInInventoryRigidbody = itemInInventory.GetComponent<Rigidbody>();
           
            _playerItemInteraction.DropItem();
            
            itemInInventoryRigidbody.AddForce(_cam.transform.forward * _throwForceForward, ForceMode.Impulse);
            itemInInventoryRigidbody.AddForce(_cam.transform.up * _throwForceUpwards, ForceMode.Impulse);

            yield return new WaitForSeconds(0.5f);
            _playerItemInteraction.isThrowingItem = false;
        }
    }
}


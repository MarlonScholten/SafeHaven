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
        [Tooltip("The force that you throw an item with.")][SerializeField] private float _throwForce = 20f;
        [Tooltip("Main camera that the player uses.")][SerializeField] private Camera _cam;
        
        public float ThrowForce => _throwForce;
        public Camera Cam => _cam;

        private Inventory _inventory;
        private PlayerItemInteraction _playerItemInteraction;
        private LineRenderer _lineRenderer;
        private DrawProjection _drawProjection;

        private void Start()
        {
            _inventory = GetComponent<Inventory>();
            _playerItemInteraction = GetComponent<PlayerItemInteraction>();
            _lineRenderer = GetComponent<LineRenderer>();
            _drawProjection = GetComponent<DrawProjection>();
            InputBehaviour.Instance.OnThrowCancelledEvent += OnThrowItem;

            if (_cam == null)
            {
                Debug.LogError("Cam in " + this + " has not been assigned!");
            }
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
            _lineRenderer.enabled = false;
            _playerItemInteraction.IsThrowingItem = true;

            var itemInInventory = _inventory.ItemInInventoryObj;
            var itemInInventoryRigidbody = itemInInventory.GetComponent<Rigidbody>();
           
            _playerItemInteraction.DropItem();

            itemInInventoryRigidbody.AddForce(_cam.transform.forward * _throwForce, ForceMode.Impulse);

            _drawProjection.DrawLine = false;
            yield return new WaitForSeconds(0.5f);
            _playerItemInteraction.IsThrowingItem = false;
        }
    }
}


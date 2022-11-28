using System;
using UnityEngine;

namespace Items
{
    public class ThrowableItemController : MonoBehaviour
    {
        [SerializeField] private float _throwForceForward;
        [SerializeField] private float _throwForceUpwards;
        [SerializeField] private Camera _cam;
        
        private Inventory _inventory;
        private PlayerItemInteraction _playerItemInteraction;

        private void Start()
        {
            _inventory = GetComponent<Inventory>();
            _playerItemInteraction = GetComponent<PlayerItemInteraction>();
        }

        private void OnThrowItem()
        {
            if (!_inventory._hasItemInInventory) return;

            if (!_inventory._itemInInventory._isThrowable)
            {
                Debug.Log("Can't Throw Item");
            }
            else ThrowItem();
        }

        private void ThrowItem()
        {
            var itemInInventory = _inventory._itemInInventoryObj;
            var itemInInventoryRigidbody = itemInInventory.GetComponent<Rigidbody>();
           
            _playerItemInteraction.DropItem();
            
            itemInInventoryRigidbody.AddForce(_cam.transform.forward * _throwForceForward, ForceMode.Impulse);
            itemInInventoryRigidbody.AddForce(_cam.transform.up * _throwForceUpwards, ForceMode.Impulse);
        }
    }
}

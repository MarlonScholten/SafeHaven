using UnityEngine;

namespace InteractableItemsSystem
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
            InputBehaviour.Instance.OnThrowEvent += OnThrowItem;
        }

        private void OnThrowItem()
        {
            if (!_inventory.HasItemInInventory) return;

            if (!_inventory.ItemInInventory.IsThrowable)
            {
                Debug.Log("Can't Throw Item");
            }
            else ThrowItem();
        }

        private void ThrowItem()
        {
            var itemInInventory = _inventory.ItemInInventoryObj;
            var itemInInventoryRigidbody = itemInInventory.GetComponent<Rigidbody>();
           
            _playerItemInteraction.DropItem();
            
            itemInInventoryRigidbody.AddForce(_cam.transform.forward * _throwForceForward, ForceMode.Impulse);
            itemInInventoryRigidbody.AddForce(_cam.transform.up * _throwForceUpwards, ForceMode.Impulse);
        }
    }
}


using System;
using System.Collections;
using UnityEngine;

namespace Items
{
    public class PlayerItemInteraction : MonoBehaviour
    {
        private Inventory _inventory;
        
        [SerializeField] private GameObject _itemHolder;
        [SerializeField] private float _maxPickupRange;

        private Transform _playerTransform;
        private ItemController _itemController;

        private float _distanceToItem;
        
        private bool _itemIsClose;
        private bool _isChangingItem;
        private bool _canPickUpItem;
        
        private RaycastHit _itemHit;
        private void Start()
        {
            _inventory = GetComponent<Inventory>();
        }
        
        private void CheckDistanceToItem()
        {
            _distanceToItem = Vector3.Distance(_playerTransform.position, _itemHit.transform.position);

            if (_distanceToItem <= _maxPickupRange && !_canPickUpItem)
            {
                _canPickUpItem = true;
                _itemHit.transform.GetComponent<ItemController>().HighlightItem(_canPickUpItem);
            }
            else if(_distanceToItem > _maxPickupRange && _canPickUpItem)
            {
                _canPickUpItem = false;
                _itemHit.transform.GetComponent<ItemController>().HighlightItem(_canPickUpItem);
            }
        }

        void OnInteractionWithItem()
        {
            if (_itemHit.transform.GetComponent<ItemController>() == null || _isChangingItem) return;
            
            if (!_inventory._hasItemInInventory && _distanceToItem < _maxPickupRange) PickUpItem();
            else if (_inventory._hasItemInInventory && _distanceToItem < _maxPickupRange) StartCoroutine(SwitchItem());
            else if (!_inventory._hasItemInInventory && _distanceToItem > _maxPickupRange) DropItem();
        }

        private IEnumerator SwitchItem()
        {
            _isChangingItem = true;
            DropItem();
            yield return new WaitForSeconds(0.5f);
            PickUpItem();
            _isChangingItem = false;
        }
        
        private void PickUpItem()
        {
            _inventory._itemInInventoryObj = _itemHit.transform.gameObject;
            
            _inventory._hasItemInInventory = true;

            _itemController = _inventory._itemInInventoryObj.GetComponent<ItemController>();
            _inventory._itemInInventory = _itemController._item;
            _itemController.PickUpItem();

            _inventory._itemInInventoryObj.transform.SetParent(_itemHolder.transform);
            _inventory._itemInInventoryObj.transform.SetPositionAndRotation(_itemHolder.transform.position,
                _itemHolder.transform.rotation);
            
            _inventory._itemHasChanged = true;
        }

        private void DropItem()
        {
            if (!_inventory._hasItemInInventory) return;
            
            _inventory._itemInInventoryObj.transform.SetParent(null);
                
            _itemController.DropItem();
                
            _inventory._itemInInventoryObj = null;
            _inventory._itemInInventory = null;

            _inventory._hasItemInInventory = false;
            _inventory._itemHasChanged = true;
        }
    }
}

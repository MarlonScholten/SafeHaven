using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    public class BrotherItemInteraction : MonoBehaviour
    {
        private Inventory _inventory;
        
        [SerializeField] private GameObject _itemHolder;
        
        private ItemController _itemController;

        private bool _itemIsClose;
        private bool _isChangingItem;

        private List<GameObject> _itemsCloseToBrother = new List<GameObject>();

        private void Start()
        {
            _inventory = GetComponent<Inventory>();
        }

        private void Update()
        {
            if (_itemsCloseToBrother.Count <= 0 && _itemIsClose)
            {
                _itemIsClose = false;
            }
        }
        
        public void OnInteractionWithItem()
        {
            if(_isChangingItem) return;
            
            if (!_inventory._hasItemInInventory && _itemIsClose) PickUpItem();
            else if (_inventory._hasItemInInventory && _itemIsClose) StartCoroutine(SwitchItem());
            else if (!_inventory._hasItemInInventory && !_itemIsClose) DropItem();
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
            int randomItem = Random.Range(0, _itemsCloseToBrother.Count - 1);
            _inventory._itemInInventoryObj = _itemsCloseToBrother[randomItem];
                
            _inventory._hasItemInInventory = true;

            _itemController = _inventory._itemInInventoryObj.GetComponent<ItemController>();
            _inventory._itemInInventory = _itemController._item;
            _itemController.PickUpItem();

            _inventory._itemInInventoryObj.transform.SetParent(_itemHolder.transform);
            _inventory._itemInInventoryObj.transform.SetPositionAndRotation(_itemHolder.transform.position,
                _itemHolder.transform.rotation);
                
            _itemsCloseToBrother.RemoveAt(randomItem);
            _inventory._itemHasChanged = true;
        }

        void DropItem()
        {
            if (!_inventory._hasItemInInventory) return;
            
            _inventory._itemInInventoryObj.transform.SetParent(null);
                
            _itemController.DropItem();
                
            _inventory._itemInInventoryObj = null;
            _inventory._itemInInventory = null;

            _inventory._hasItemInInventory = false;
            _inventory._itemHasChanged = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ItemController>() != null)
            {
                _itemsCloseToBrother.Add(other.gameObject);
                _itemIsClose = true;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ItemController>() != null)
            {
                _itemsCloseToBrother.Remove(other.gameObject);
            }
        }
    }
}
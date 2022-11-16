using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Items
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Image _inventoryUI;
        [SerializeField] private GameObject _itemHolder;
        
        private GameObject _itemInInventory;
        private Item _itemI;
        private ItemController _itemController;

        private bool _itemIsClose;
        private bool _hasItemInInventory;
        private bool _isChangingItem;
        private bool _itemHasChanged;

        private List<GameObject> _itemsClose = new List<GameObject>();

        private void Start()
        {
            ChangeInventoryUI();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.E)) OnPickup();
            if (Input.GetKeyDown(KeyCode.Q)) OnDrop();

            if (_itemsClose.Count <= 0 && _itemIsClose)
            {
                _itemIsClose = false;
            }

            if (_itemHasChanged) ChangeInventoryUI();
        }
        
        void OnPickup()
        {
            if (_itemIsClose && !_isChangingItem)
            {
                if (!_hasItemInInventory) PickUpitem();
                else StartCoroutine(SwitchItem());
            }
        }
        
        IEnumerator SwitchItem()
        {
            _isChangingItem = true;
            OnDrop();
            yield return new WaitForSeconds(0.5f);
            PickUpitem();
            _isChangingItem = false;
        }
        
        public void PickUpitem()
        {
            int randomItem = Random.Range(0, _itemsClose.Count - 1);
            _itemInInventory = _itemsClose[randomItem];
                
            _hasItemInInventory = true;

            _itemController = _itemInInventory.GetComponent<ItemController>();
            _itemI = _itemController._item;
            _itemController.PickUpItem();

            _itemInInventory.transform.SetParent(_itemHolder.transform);
            _itemInInventory.transform.SetPositionAndRotation(_itemHolder.transform.position,
                _itemHolder.transform.rotation);
                
            _itemsClose.RemoveAt(randomItem);
            _itemHasChanged = true;
        }

        void OnDrop()
        {
            if (_hasItemInInventory)
            {
                _itemInInventory.transform.SetParent(null);
                
                _itemController.DropItem();
                
                _itemInInventory = null;
                _itemI = null;

                _hasItemInInventory = false;
                _itemHasChanged = true;
            }
        }

        public void ChangeInventoryUI()
        {
            if (_hasItemInInventory)
            {
                _inventoryUI.sprite = _itemI._icon;
                _inventoryUI.enabled = true;
            }
            else
            {
                _inventoryUI.enabled = false;
                _inventoryUI.sprite = null;
            }

            _itemHasChanged = false;
        }
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ItemController>() != null)
            {
                _itemsClose.Add(other.gameObject);
                _itemIsClose = true;
            }
        }
        
        /// <summary>
        ///When an item enters player pickup range,
        /// then the item will be added to a list with items that are close to the player.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ItemController>() != null)
            {
                _itemsClose.Remove(other.gameObject);
            }
        }
    }
}

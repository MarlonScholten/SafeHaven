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
        
        private GameObject _itemInInventoryOBJ;
        public Item _itemInInventory;
        private ItemController _itemController;

        private bool _itemIsClose;
        public bool _hasItemInInventory;
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
            _itemInInventoryOBJ = _itemsClose[randomItem];
                
            _hasItemInInventory = true;

            _itemController = _itemInInventoryOBJ.GetComponent<ItemController>();
            _itemInInventory = _itemController._item;
            _itemController.PickUpItem();

            _itemInInventoryOBJ.transform.SetParent(_itemHolder.transform);
            _itemInInventoryOBJ.transform.SetPositionAndRotation(_itemHolder.transform.position,
                _itemHolder.transform.rotation);
                
            _itemsClose.RemoveAt(randomItem);
            _itemHasChanged = true;
        }

        void OnDrop()
        {
            if (_hasItemInInventory)
            {
                _itemInInventoryOBJ.transform.SetParent(null);
                
                _itemController.DropItem();
                
                _itemInInventoryOBJ = null;
                _itemInInventory = null;

                _hasItemInInventory = false;
                _itemHasChanged = true;
            }
        }

        public void ChangeInventoryUI()
        {
            if (_hasItemInInventory)
            {
                _inventoryUI.sprite = _itemInInventory._icon;
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

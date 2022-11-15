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
        
        [SerializeField] private GameObject _itemInInventory;
        [SerializeField] private Item _itemI;
        private ItemController _itemController;

        [SerializeField] private bool _itemIsClose;
        [SerializeField] private bool _hasItemInInventory;

        public List<GameObject> _itemsClose;
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.E)) OnPickup();
            if (Input.GetKeyDown(KeyCode.Q)) OnDrop();

            if (_itemsClose.Count <= 0 && _itemIsClose)
            {
                _itemIsClose = false;
            }
        }
        
        void OnPickup()
        {
            if (_itemIsClose)
            {
                if (!_hasItemInInventory) PickUpitem();
                else StartCoroutine(SwitchItem());
            }
        }
        
        IEnumerator SwitchItem()
        {
            OnDrop();
            yield return new WaitForSeconds(0.5f);
            PickUpitem();
        }
        
        public void PickUpitem()
        {
            int randomItem = Random.Range(0, _itemsClose.Count - 1);
            _itemInInventory = _itemsClose[randomItem];
                
            _hasItemInInventory = true;

            _itemController = _itemInInventory.GetComponent<ItemController>();
            _itemI = _itemController._item;
            _itemController.PickUpItem();

            _itemInInventory.transform.parent = _itemHolder.transform;
            _itemInInventory.transform.position = _itemHolder.transform.position;
            _itemInInventory.transform.rotation = _itemHolder.transform.rotation;
                
            _itemsClose.RemoveAt(randomItem);
        }

        void OnDrop()
        {
            if (_hasItemInInventory)
            {
                _itemInInventory.transform.parent = _itemInInventory.transform;
                
                _itemController.DropItem();
                
                _itemInInventory = null;
                _itemI = null;

                _hasItemInInventory = false;
            }
        }
        
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ItemController>() != null)
            {
                _itemsClose.Add(other.gameObject);
                _itemIsClose = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ItemController>() != null)
            {
                _itemsClose.Remove(other.gameObject);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Player_Character.Interactable_Items_System;
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
        
        public GameObject _itemInInventoryObj;
        public Item _itemInInventory;
        public bool _hasItemInInventory;
        public bool _itemHasChanged;

        private PlayerItemInteraction _playerItemInteraction;


        private void Start()
        {
            _playerItemInteraction = GetComponent<PlayerItemInteraction>();
            ChangeInventoryUI();
        }

        private void Update()
        {
            if (_itemHasChanged) ChangeInventoryUI();
        }

        private void ChangeInventoryUI()
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
    }
}

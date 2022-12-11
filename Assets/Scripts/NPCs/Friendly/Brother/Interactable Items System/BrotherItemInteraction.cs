using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


    namespace InteractableItemsSystem
    {
        /// <summary>
        /// Author: Jasper Driessen <br/>
        /// Modified by:  <br/>
        /// Description: This script is used for making it so the brother can interact with items.
        /// Pickup, drop and switch items.
        /// 1. Drag the "BrotherAI Interact System" prefab under the BrotherAI GameObject.
        /// The BrotherItemInteraction should already be on this prefab.
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
        ///         <term>BrotherAI Interact System</term>
        ///         <term>Used for controlling the interaction with items of the BrotherAI.</term>
        ///     </item>
        /// </list>
        public class BrotherItemInteraction : MonoBehaviour
        {
            [Tooltip("The GameObject where the item in the brothers inventory will be placed under.")][SerializeField] 
            private GameObject _itemHolder;

            private bool _itemIsClose;
            private bool _isChangingItem;

            private Inventory _inventory;
            private ItemController _itemController;
            
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
            
                if (!_inventory.HasItemInInventory && _itemIsClose) PickUpItem();
                else if (_inventory.HasItemInInventory && _itemIsClose) StartCoroutine(SwitchItem());
                else if (!_inventory.HasItemInInventory && !_itemIsClose) DropItem();
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
                _inventory.ItemInInventoryObj = _itemsCloseToBrother[randomItem];
                
                _inventory.HasItemInInventory = true;

                _itemController = _inventory.ItemInInventoryObj.GetComponent<ItemController>();
                _inventory.ItemInInventory = _itemController.Item;
                _itemController.PickUpItem();

                _inventory.ItemInInventoryObj.transform.SetParent(_itemHolder.transform);
                _inventory.ItemInInventoryObj.transform.SetPositionAndRotation(_itemHolder.transform.position,
                    _itemHolder.transform.rotation);
                
                _itemsCloseToBrother.RemoveAt(randomItem);
                _inventory.ItemHasChanged = true;
            }

            void DropItem()
            {
                if (!_inventory.HasItemInInventory) return;
            
                _inventory.ItemInInventoryObj.transform.SetParent(null);
                
                _itemController.DropItem();
                
                _inventory.ItemInInventoryObj = null;
                _inventory.ItemInInventory = null;

                _inventory.HasItemInInventory = false;
                _inventory.ItemHasChanged = true;
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

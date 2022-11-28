using UnityEngine;
using UnityEngine.UI;


namespace InteractableItemsSystem
{
    /// <summary>
    /// Author: Jasper Driessen <br/>
    /// Modified by:  <br/>
    /// Description: Saves the item the player is holding. <br/>
    /// 1. Drag the 
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///     <item>
    ///         <term>Interactable</term>
    ///         <term>TYPE</term>
    ///         <term>NAME</term>
    ///         <term>DISCRIPTION</term>
    ///     </item>
    /// </list>
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Image _inventoryUI;

        public GameObject ItemInInventoryObj;
        public ItemSO ItemInInventory;
        public bool HasItemInInventory;
        public bool ItemHasChanged;

        private PlayerItemInteraction _playerItemInteraction;


        private void Start()
        {
            _playerItemInteraction = GetComponent<PlayerItemInteraction>();
            ChangeInventoryUI();
        }

        private void Update()
        {
            if (ItemHasChanged) ChangeInventoryUI();
        }

        private void ChangeInventoryUI()
        {
            if (HasItemInInventory)
            {
                _inventoryUI.sprite = ItemInInventory.Icon;
                _inventoryUI.enabled = true;
            }
            else
            {
                _inventoryUI.enabled = false;
                _inventoryUI.sprite = null;
            }

            ItemHasChanged = false;
        }
    }
}


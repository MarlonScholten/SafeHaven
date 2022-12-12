using System;
using UnityEngine;
using UnityEngine.UI;


namespace InteractableItemsSystem
{
    /// <summary>
    /// Author: Jasper Driessen <br/>
    /// Modified by:  <br/>
    /// Description: Saves the item the player is holding. <br/>
    /// 1. Drag the "Player Interact System" prefab under the player
    /// or when doing this for the brother the "Brother Interact System" prefab under the BrotherAI.
    /// (this should already contain the inventory script)
    /// 2. Drag the "Global Inventory UI" prefab under the canvas. (If not already done)
    /// 3. Drag the corresponding Item UI in the "Inventory Item UI" in the script.
    /// You can find this under Canvas/GlobalInventoryUI/InventoryUI/ItemUI.
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
    ///         <term>Player Interact System</term>
    ///         <term>Used for interaction with items for the player.</term>
    ///     </item>
    ///     <item>
    ///         <term>BrotherAI GameObject</term>
    ///         <term>Prefab</term>
    ///         <term>Brother Interact System</term>
    ///         <term>Used for interaction with items for the brother.</term>
    ///     </item>
    ///     <item>
    ///         <term>Canvas</term>
    ///         <term>Prefab</term>
    ///         <term>Global Inventory UI</term>
    ///         <term>Used for changing the sprite of the UI to what item the player is holding.</term>
    ///     </item>
    /// </list>
    public class Inventory : MonoBehaviour
    {
        [Tooltip("UI element of the item the player is holding.")][SerializeField] private Image _inventoryItemUI;

        //These public variables change it other scripts.
        /// <summary>
        /// The GameObject of the Item that is in the inventory.
        /// </summary>
        [NonSerialized] public GameObject ItemInInventoryObj;
        /// <summary>
        /// The Item (data about the item) that is in the inventory.
        /// </summary>
        [NonSerialized] public ItemSO ItemInInventory;
        /// <summary>
        /// Determines if there is an item in the inventory.
        /// </summary>
        [NonSerialized] public bool HasItemInInventory;
        /// <summary>
        /// Determines if the state of the inventory has changed. (If there is an item dropped, pickup, thrown or switched).
        /// </summary>
        [NonSerialized] public bool ItemHasChanged;


        private void Start()
        {
            ChangeInventoryUI();

            if (_inventoryItemUI == null)
            {
                Debug.LogError("Inventory Item UI in "  + this + " has not been assigned!");
            }
        }

        private void Update()
        {
            if (ItemHasChanged) ChangeInventoryUI();
        }

        private void ChangeInventoryUI()
        {
            if (_inventoryItemUI != null)
            {
                if (HasItemInInventory)
                {
                    if(ItemInInventory.Icon == null)
                    {
                        Debug.LogError(ItemInInventory + " doens't have an icon assigned to it!");
                    }
                    else
                    {
                        _inventoryItemUI.sprite = ItemInInventory.Icon;
                        _inventoryItemUI.enabled = true;
                    }
                }
                else
                {
                    _inventoryItemUI.enabled = false;
                    _inventoryItemUI.sprite = null;
                }
            }
            ItemHasChanged = false;
        }
    }
}


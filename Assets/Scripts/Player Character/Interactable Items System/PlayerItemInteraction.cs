using System;
using System.Collections;
using PlayerCharacter.Movement;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace InteractableItemsSystem
{
    /// <summary>
    /// Author: Jasper Driessen <br/>
    /// Modified by:  <br/>
    /// Description: This script is used for making it so the player can interact with other object/items.
    /// Pickup, drop and switch items.
    /// 1. Drag the "Player Interact System" prefab under the player GameObject.
    /// The PlayerItemInteraction should already be on this prefab.
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
    ///         <term>Used for controlling the interaction with items of the player.</term>
    ///     </item>
    ///     <item>
    ///         <term>Player GameObject</term>
    ///         <term>Script</term>
    ///         <term>PlayerController</term>
    ///         <term>Used for controlling the movement of the player.</term>
    ///     </item>
    ///     <item>
    ///         <term>Player Interact System</term>
    ///         <term>Script</term>
    ///         <term>Inventory</term>
    ///         <term>Inventory for the player where the item the player is holding will be stored.</term>
    ///     </item>
    ///       <item>
    ///         <term>Item GameObject</term>
    ///         <term>Script</term>
    ///         <term>Item Controller</term>
    ///         <term>Used for storing the data of an item and controller the pickup and drop on the item.</term>
    ///     </item>
    ///      <item>
    ///         <term>Interactable Object GameObject</term>
    ///         <term>Script</term>
    ///         <term>InteractableObject</term>
    ///         <term>Used for making interactions between object and items possible.</term>
    ///     </item>
    /// </list>
    [RequireComponent(typeof(Inventory))]
    public class PlayerItemInteraction : MonoBehaviour
    {
        [Tooltip("Interaction prompt for interacting with items or objects")] [SerializeField] TextMeshProUGUI _actionPromptText;
        [Tooltip("The GameObject where the item in your inventory will be placed under.")][SerializeField] 
        private GameObject _itemHolder;
        [Tooltip("Max range that you can pickup an item.")][SerializeField] 
        private float _maxPickupRange = 3f;
        [Tooltip("Max range that you can interact with an other Object.")][SerializeField] 
        private float _maxInteractRange = 2f;
        [Tooltip("Color of the emission of the highlighted object")][SerializeField] private Color _highlightColor = new Color32(41, 41, 41, 0);

        
        //Gets changed in ThrowableItemController script
        /// <summary>
        /// Determines if a player is throwing an item.
        /// </summary>
        [NonSerialized] public bool IsThrowingItem;
        /// <summary>
        /// The item holder of the player. GameObject of where the item GameObject is stored.
        /// </summary>
        public GameObject ItemHolder => _itemHolder;
        /// <summary>
        /// Color for highlighting an object or item.
        /// </summary>
        public Color HighlightColor => _highlightColor;

        private float _distanceToItem;

        private bool _isChangingItem;
        private bool _closeToInteractableObject;
        private bool _closeToItem;

        private RaycastHit _itemHit;
        private Transform _playerTransform;
        private Renderer _rendererItem;
        
        private Inventory _inventory;
        private ItemController _itemController;
        private InteractableObject _interactableObject;
        private PlayerController _playerController;
        
        private void Start()
        {
            _inventory = GetComponent<Inventory>();
            _playerTransform = transform.parent;
            _playerController = _playerTransform.GetComponent<PlayerController>();
            InputBehaviour.Instance.OnItemInteractEvent += OnInteractionWithItem;
            _itemHit = _playerController.CamRayCastHit;
        }

        private void Update()
        {
            CheckDistanceToItem();
            UpdateActionPromptText();
        }

        private void CheckDistanceToItem()
        {
            if (_rendererItem != null)
            {
                _rendererItem.material.DisableKeyword("_EMISSION");
                _rendererItem = null;
            }
            
            _itemHit = _playerController.CamRayCastHit;
            if (_itemHit.transform == null || _isChangingItem || IsThrowingItem) return;

            _distanceToItem = Vector3.Distance(_playerTransform.position, _itemHit.point);
            
            if (_itemHit.transform.GetComponent<ItemController>() != null)
            {
                var itemController = _itemHit.transform.GetComponent<ItemController>();
                var renderer = itemController.transform.GetComponent<Renderer>();

                if (_distanceToItem <= _maxPickupRange)
                {
                    _closeToItem = true;
                    
                    renderer.material.EnableKeyword("_EMISSION");
                    renderer.material.SetColor("_EmissionColor", _highlightColor);
                    _rendererItem = renderer;
                }
                else
                {
                    renderer.material.DisableKeyword("_EMISSION");
                }
            }
            else if (_itemHit.transform.GetComponent<InteractableObject>() != null)
            {
                var interactableObject = _itemHit.transform.GetComponent<InteractableObject>();
                var renderer = interactableObject.transform.GetComponent<Renderer>();

                if (_distanceToItem <= _maxPickupRange)
                {
                    _closeToInteractableObject = true;
                    
                    renderer.material.EnableKeyword("_EMISSION");
                    renderer.material.SetColor("_EmissionColor", _highlightColor);
                    _rendererItem = renderer;
                }
                else
                {
                    renderer.material.DisableKeyword("_EMISSION");
                }
            }
        }

        void UpdateActionPromptText()
        {
            if (_actionPromptText == null)
            {
                Debug.LogError("Action Prompt Text in " + this + " has not been assigned!");
                return;
            }
            if (_closeToItem)
            {
                _actionPromptText.enabled = true;
                _actionPromptText.text = "Press E to pickup";
                
                _closeToItem = false;
            }
            else if(_closeToInteractableObject)
            {
                _actionPromptText.enabled = true;
                _actionPromptText.text = "Press E to interact";
                
                _closeToInteractableObject = false;
            }
            else if (_inventory.HasItemInInventory)
            {
                _actionPromptText.enabled = true;
                _actionPromptText.text = "Press E to drop";
            }
            else
            {
                _actionPromptText.enabled = false;
            }
        }

        private void OnInteractionWithItem()
        {
            if(_isChangingItem || IsThrowingItem) return;
            
            if (_itemHit.transform.GetComponent<InteractableObject>() != null && _distanceToItem <= _maxInteractRange)
            {
                _interactableObject = _itemHit.transform.GetComponent<InteractableObject>();
                TryInteraction();
            }
            else if (_itemHit.transform.GetComponent<ItemController>() != null)
            {
                if (!_inventory.HasItemInInventory && _distanceToItem <= _maxPickupRange) PickUpItem(_itemHit.transform.gameObject);
                else if (_inventory.HasItemInInventory && _distanceToItem <= _maxPickupRange) StartCoroutine(SwitchItem(_itemHit.transform.gameObject));
                else if (_inventory.HasItemInInventory && _distanceToItem > _maxPickupRange) DropItem();
            }
            else
            {
                if (_inventory.HasItemInInventory) DropItem();
            }
        }

        private IEnumerator SwitchItem(GameObject itemHitGameObject)
        {
            _isChangingItem = true;
            DropItem();
            yield return new WaitForSeconds(0.5f);
            PickUpItem(itemHitGameObject);
            _isChangingItem = false;
        }
        
        private void PickUpItem(GameObject itemHitGameObject)
        {
            _inventory.ItemInInventoryObj = itemHitGameObject;
            
            _inventory.HasItemInInventory = true;

            _itemController = _inventory.ItemInInventoryObj.GetComponent<ItemController>();
            _inventory.ItemInInventory = _itemController.Item;
            _itemController.PickUpItem();

            _inventory.ItemInInventoryObj.transform.SetParent(_itemHolder.transform);
            _inventory.ItemInInventoryObj.transform.SetPositionAndRotation(_itemHolder.transform.position,
                _itemHolder.transform.rotation);
            
            _inventory.ItemHasChanged = true;
        }

        /// <summary>
        /// Drops the item the player is currently holding in his inventory.
        /// </summary>
        public void DropItem()
        {
            if (!_inventory.HasItemInInventory) return;
            
            _inventory.ItemInInventoryObj.transform.SetParent(null);
                
            _itemController.DropItem();
                
            _inventory.ItemInInventoryObj = null;
            _inventory.ItemInInventory = null;

            _inventory.HasItemInInventory = false;
            _inventory.ItemHasChanged = true;
        }
        
        private void TryInteraction()
        {
            if (!_inventory.HasItemInInventory)
            {
                _interactableObject.CanNotInteractWithObjectNoItemEvent?.Invoke();
                return;
            }
            if (_interactableObject.ItemNeededToInteract.Sort == _inventory.ItemInInventory.Sort)
            {
                if (_interactableObject.NameImportant)
                {
                    if (_interactableObject.ItemNeededToInteract.Name == _inventory.ItemInInventory.Name) _interactableObject.InteractWithObjectEvent?.Invoke();
                    else _interactableObject.CanNotInteractWithObjectWrongNameEvent?.Invoke();
                }
                else _interactableObject.InteractWithObjectEvent?.Invoke();
            }
            else _interactableObject.CanNotInteractWithObjectWrongItemEvent?.Invoke();
        }
    }
}

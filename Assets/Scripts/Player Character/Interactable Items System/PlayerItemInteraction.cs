using System.Collections;
using Player_Character.Player_Movement.State_machine.State_machines;
using UnityEngine;

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
    /// </list>
    public class PlayerItemInteraction : MonoBehaviour
    {
        private Inventory _inventory;

        [Tooltip("The GameObject where the item in your inventory will be placed under.")][SerializeField] 
        private GameObject _itemHolder;
        [Tooltip("Max range that you can pickup an item.")][SerializeField] 
        private float _maxPickupRange;
        [Tooltip("Max range that you can interact with an other Object.")][SerializeField] 
        private float _maxInteractRange; 
        
        //Tempcode only for debugging and to stop spam.
        [Tooltip("Only for debugging, spams debug.log, if you can pickup or interact with an item.")] [SerializeField] 
        private bool _showSpamDebug;
        
        private Transform _playerTransform;
        private ItemController _itemController;
        private InteractableObject _interactableObject;
        private PlayerController _playerController;

        private float _distanceToItem;

        private bool _itemIsClose;
        private bool _isChangingItem;

        private RaycastHit _itemHit;
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
        }

        private void CheckDistanceToItem()
        {
            _itemHit = _playerController.CamRayCastHit;

            //Should be looked further in to, how this can be made better.
            if (_itemHit.transform.GetComponent<ItemController>() == null &&
                _itemHit.transform.GetComponent<InteractableObject>() == null) return;
            
            _distanceToItem = Vector3.Distance(_playerTransform.position, _itemHit.point);

            //Temp code should change to material highlight or UI element.
            if (_itemHit.transform.GetComponent<ItemController>() != null)
            {
                if (_distanceToItem <= _maxPickupRange && _showSpamDebug)
                {
                    Debug.Log("Can PickUp the Item!");
                }
                else if(_distanceToItem > _maxPickupRange && _showSpamDebug)
                {
                    Debug.Log("Looks at an Item!");
                }
            }
            else
            {
                if (_distanceToItem <= _maxPickupRange && _showSpamDebug)
                {
                    Debug.Log("Can Interact with Object!");
                }
                else if(_distanceToItem > _maxPickupRange && _showSpamDebug)
                {
                    Debug.Log("Looks at Interactable Object!");
                }
            }
        }

        private void OnInteractionWithItem()
        {
            if(_isChangingItem) return;
            
            if (_itemHit.transform.GetComponent<InteractableObject>() != null)
            {
                _interactableObject = _itemHit.transform.GetComponent<InteractableObject>();
                if(_distanceToItem < _maxInteractRange)TryInteraction();
            }
            else if (_itemHit.transform.GetComponent<ItemController>() != null)
            {
                if (!_inventory.HasItemInInventory && _distanceToItem < _maxPickupRange) PickUpItem();
                else if (_inventory.HasItemInInventory && _distanceToItem < _maxPickupRange) StartCoroutine(SwitchItem());
                else if (!_inventory.HasItemInInventory && _distanceToItem > _maxPickupRange) DropItem();
            }
            else
            {
                if (_inventory.HasItemInInventory) DropItem();
            }
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
            _inventory.ItemInInventoryObj = _itemHit.transform.gameObject;
            
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
                CanNotInteractWithObject("WrongItem");
                return;
            }
            if (_interactableObject.ItemNeededToInteract.Sort == _inventory.ItemInInventory.Sort)
            {
                if (_interactableObject.NameImportant)
                {
                    if (_interactableObject.ItemNeededToInteract.Name == _inventory.ItemInInventory.Name) InteractWithObject();
                    else CanNotInteractWithObject("WrongName");
                }
                else InteractWithObject();
            }
            else CanNotInteractWithObject("WrongItem");
        }
        
        private static void InteractWithObject()
        {
            Debug.Log("Interacted");
        }
        
        private static void CanNotInteractWithObject(string why)
        {
            switch (why)
            {
                case "WrongItem":
                    Debug.Log("WrongItem");
                    break;
                case "WrongName":
                    Debug.Log("WrongName");
                    break;
            }
        }
    }
}

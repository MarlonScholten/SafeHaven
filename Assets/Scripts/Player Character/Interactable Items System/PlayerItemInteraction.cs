using System.Collections;
using Items;
using Items.Interactable_Objects;
using Player_Character.Player_Movement.State_machine.State_machines;
using UnityEngine;

namespace Player_Character.Interactable_Items_System
{
    public class PlayerItemInteraction : MonoBehaviour
    {
        private Inventory _inventory;
        
        [SerializeField] private GameObject _itemHolder;
        [SerializeField] private float _maxPickupRange;
        [SerializeField] private float _maxInteractRange; 
        
        private Transform _playerTransform;
        private ItemController _itemController;
        private InteractableObject _interactableObject;
        private PlayerController _playerController;

        private float _distanceToItem;

        private bool _itemIsClose;
        private bool _isChangingItem;
        private bool _canPickUpItem;
        
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
            
            if(_itemHit.transform.GetComponent<ItemController>() == null) return;
            _distanceToItem = Vector3.Distance(_playerTransform.position, _itemHit.point);
            
            if (_distanceToItem <= _maxPickupRange)
            {
                _canPickUpItem = true;
                Debug.Log(_canPickUpItem);
                /*_itemHit.transform.GetComponent<ItemController>().HighlightItem(_canPickUpItem);*/
            }
            else if(_distanceToItem > _maxPickupRange)
            {
                _canPickUpItem = false;
                Debug.Log(_canPickUpItem);
                /*_itemHit.transform.GetComponent<ItemController>().HighlightItem(_canPickUpItem);*/
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
                if (!_inventory._hasItemInInventory && _distanceToItem < _maxPickupRange) PickUpItem();
                else if (_inventory._hasItemInInventory && _distanceToItem < _maxPickupRange) StartCoroutine(SwitchItem());
                else if (!_inventory._hasItemInInventory && _distanceToItem > _maxPickupRange) DropItem();
            }
            else
            {
                if (_inventory._hasItemInInventory) DropItem();
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
            _inventory._itemInInventoryObj = _itemHit.transform.gameObject;
            
            _inventory._hasItemInInventory = true;

            _itemController = _inventory._itemInInventoryObj.GetComponent<ItemController>();
            _inventory._itemInInventory = _itemController._item;
            _itemController.PickUpItem();

            _inventory._itemInInventoryObj.transform.SetParent(_itemHolder.transform);
            _inventory._itemInInventoryObj.transform.SetPositionAndRotation(_itemHolder.transform.position,
                _itemHolder.transform.rotation);
            
            _inventory._itemHasChanged = true;
        }

        public void DropItem()
        {
            if (!_inventory._hasItemInInventory) return;
            
            _inventory._itemInInventoryObj.transform.SetParent(null);
                
            _itemController.DropItem();
                
            _inventory._itemInInventoryObj = null;
            _inventory._itemInInventory = null;

            _inventory._hasItemInInventory = false;
            _inventory._itemHasChanged = true;
        }
        
        private void TryInteraction()
        {
            if (!_inventory._hasItemInInventory)
            {
                CanNotInteractWithObject("WrongItem");
                return;
            }
            if (_interactableObject._itemNeededToInteract._sort == _inventory._itemInInventory._sort)
            {
                if (_interactableObject._nameImportant)
                {
                    if (_interactableObject._itemNeededToInteract._name == _inventory._itemInInventory._name) InteractWithObject();
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

using System;
using UnityEngine;

namespace Items.Interactable_Objects
{
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] private Item _itemNeededToInteract;
        [SerializeField] private bool _nameImportant = false;

        [SerializeField] private bool _playerClose;
        [SerializeField] private Inventory _inventory;

        private void OnTriggerEnter(Collider other)
        {
            _inventory = other.GetComponentInChildren<Inventory>();
            if (_inventory != null)
            {
                _playerClose = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _inventory = null;
            _playerClose = false;
        }

        // Update is called once per frame
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.F)) TryInteraction();
        }

        private void TryInteraction()
        {
            if (!_playerClose) return;
            if (!_inventory._hasItemInInventory)
            {
                CanNotInteract("NoKey");
                return;
            }
            if (_itemNeededToInteract._sort == _inventory._itemInInventory._sort)
            {
                if (_nameImportant)
                {
                    if (_itemNeededToInteract._name == _inventory._itemInInventory._name)
                    {
                        Interact();
                    }
                    else
                    {
                        CanNotInteract("WrongKey");
                    }
                }
                else
                {
                    Interact();
                }
            }
            else
            {
                CanNotInteract("NoKey");
            }
        }

        private static void Interact()
        {
            //TempCode
            Debug.Log("Door opened");
        }

        private static void CanNotInteract(string why)
        {
            //TempCode
            if (why == "WrongKey")
            {
                Debug.Log("This key doesn't fit");
            }
            else if (why == "NoKey")
            {
                Debug.Log("I need a key");
            }
        }
    }
}

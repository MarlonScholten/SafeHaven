using System;
using UnityEngine;

namespace Items.Interactable_Objects
{
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] public Item _itemNeededToInteract;
        [SerializeField] public bool _nameImportant = false;
    }
}

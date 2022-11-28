using UnityEngine;


namespace InteractableItemsSystem
{
    
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] private ItemSO _itemNeededToInteract;
        [SerializeField] private bool _nameImportant;
        
        public ItemSO ItemNeededToInteract => _itemNeededToInteract;
        /// <summary>
        /// Wow verry could thing
        /// </summary>
        public bool NameImportant => _nameImportant;
    }
}


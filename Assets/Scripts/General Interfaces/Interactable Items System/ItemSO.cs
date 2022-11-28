using UnityEngine;

namespace InteractableItemsSystem
{
    /// <summary>
    /// Author: Jasper Driessen <br/>
    /// Modified by:  <br/>
    /// Description: This is a scriptable object that holds the information about the Item.
    /// Create a new item in the assets menu (Item/Create New Item)
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///     <item>
    ///         <term>The GameObject of your item</term>
    ///         <term>SerializedField of Script</term>
    ///         <term>ItemController - Item</term>
    ///         <term>This scriptable object needs to be added, otherwise the ItemController doesn't have data for the items.</term>
    ///     </item>
    ///     <item>
    ///         <term>The GameObject of your interactable object</term>
    ///         <term>SerializedField of Script</term>
    ///         <term>Interactable Object - Item</term>
    ///         <term>This scriptable object needs to be added, otherwise the Interactable Object doesn't have data
    ///         for which item should be used to interact with the object. </term>
    ///     </item>
    /// </list>
    
    [CreateAssetMenu(fileName ="New Item", menuName ="Item/Create New Item")]
    public class ItemSO : ScriptableObject
    {
        [Tooltip("Name of the item.")][SerializeField] private string _name;
        [Tooltip("Sort of item. Like a key for example")][SerializeField] private string _sort;
        [Tooltip("Icon that is used in the inventory UI for showing which item you're holding")][SerializeField] private Sprite _icon;
        [Tooltip("Determines if item is throwable.")][SerializeField] private bool _isThrowable;
        
        public string Name => _name;
        public string Sort => _sort;
        public Sprite Icon => _icon;
        public bool IsThrowable => _isThrowable;
    }
}

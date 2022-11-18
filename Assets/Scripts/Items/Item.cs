using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName ="New Item", menuName ="Item/Create New Item")]
    public class Item : ScriptableObject
    {
        [Tooltip("The ID of the item.")]public int _id;
        [Tooltip("The name of the item.")]public string _name;
        [Tooltip("What kind of item is it. For example a key.")]public string _sort;
        [Tooltip("The icon of the item. Will be used in UI inventory.")]public Sprite _icon;
    }
}

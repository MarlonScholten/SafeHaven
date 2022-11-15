using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName ="New Item", menuName ="Item/Create New Item")]
    public class Item : ScriptableObject
    {
        public int _id;
        public string _name;
        public int _value;
        public Sprite _icon;
    }
}

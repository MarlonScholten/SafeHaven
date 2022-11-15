using System;
using UnityEditor;
using UnityEngine;

namespace Items
{
    public class ItemController : MonoBehaviour
    {
        public Item _item;

        private Rigidbody _rb;
        private Collider _col;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _col = GetComponent<Collider>();
        }

        public void PickUpItem()
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;

            _col.enabled = false;
        }
        
        public void DropItem()
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
            
            _col.enabled = true;
        }
    }
}

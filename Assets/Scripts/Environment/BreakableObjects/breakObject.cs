using System;
using UnityEngine;
using UnityEngine.Events;

namespace Environment.BreakableObjects
{
    public class breakObject : MonoBehaviour
    {
        [SerializeField] private float thresholdToBreak = 0.5f;
    
        [NonSerialized] public UnityEvent objectIsBroken;

        public breakObject(UnityEvent objectIsBroken)
        {
            this.objectIsBroken = objectIsBroken;
        }

        // Start is called before the first frame update
        void Start()
        {
       
        }

        // Update is called once per frame
        void Update()
        {
            // if(Animation/Sound is finished)
            // {
            //     Destroy(gameObject);
            // }
        }
    
        void OnCollisionEnter(Collision col) {
            if(col.relativeVelocity.magnitude > thresholdToBreak) {
                objectIsBroken.Invoke();
            }
        }
    }
}

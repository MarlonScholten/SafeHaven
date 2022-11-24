using UnityEngine;

namespace NPC
{
    /// <summary>
    /// <br>Author: Marlon Kerstens</br>
    /// <br>Modified by: Hugo Ulfman</br>
    /// Description: This script is that is for test purposes only. It is used to simulate a sound source.
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///	    <item>
    ///         <term>Any object</term>
    ///		    <term>Component</term>
    ///         <term>Any object</term>
    ///		    <term>This script is used to simulate a soundSource on the object that it is placed. The sound can be simulated by pressing spacebar.</term>
    ///	    </item>
    /// </list>
    public class MakeNoise : MonoBehaviour
    {
        [Tooltip("The noise level of the object")]
        [SerializeField] private float noiseLevel = 10f;

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        void Update()
        {
            //when space bar pressed, make noise
            if (Input.GetKeyDown(KeyCode.Space))
            {
                makeNoise();
            }
        }
        
        /// <summary>
        /// This method creates a noise source on the object that this script is attached to.
        /// </summary>
        private void makeNoise(){
            var foundObjects = Physics.OverlapSphere(transform.position, noiseLevel/2);
            foreach(var currentObject in foundObjects)
            {
                if (!currentObject.CompareTag("Enemy")) continue;
                var distance = Vector3.Distance(transform.position, currentObject.transform.position);
                var source = new SoundSource(gameObject, noiseLevel / distance);
                currentObject.GetComponent<PatrolState>().HeardASoundEvent.Invoke(source);
            }
        }
    }
}

using UnityEngine;

namespace NPC
{
    public class MakeNoise : MonoBehaviour
    {
        public float noiseLevel = 10f;

        // Update is called once per frame
        void Update()
        {
            //when space bar pressed, make noise
            if (Input.GetKeyDown(KeyCode.Space))
            {
                makeNoise();
            }
        }

        private void makeNoise(){
            var foundObjects = Physics.OverlapSphere(transform.position, noiseLevel/2);
            foreach(var currentObject in foundObjects)
            {
                if (!currentObject.CompareTag("Enemy")) continue;
                var distance = Vector3.Distance(transform.position, currentObject.transform.position);
                var source = new SoundSource(gameObject, noiseLevel / distance);
                currentObject.GetComponent<Enemy_Finite_State_Machine>().HeardASoundEvent.Invoke(source);
            }
        }
    }
}

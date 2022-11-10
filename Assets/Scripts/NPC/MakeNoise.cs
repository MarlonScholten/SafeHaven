using System.Collections;
using System.Collections.Generic;
using NPC;
using UnityEngine;

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
    
    void makeNoise(){
        Collider[] foundObjects = Physics.OverlapSphere(transform.position, noiseLevel/2);
        foreach(var currentObject in foundObjects)
        {
            if (currentObject.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, currentObject.transform.position);
                SoundSource source = new SoundSource(gameObject, noiseLevel / distance);
                currentObject.transform.gameObject.SendMessage("NoiseReceived", source,
                    SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}

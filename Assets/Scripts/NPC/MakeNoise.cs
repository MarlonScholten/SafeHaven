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
            spawnSound();         
            makeNoise();
        }
    }

    void spawnSound(){
        GameObject sound = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        sound.transform.position = transform.position;
        sound.transform.Translate(0, -0.5f, 0);
        sound.transform.localScale = new Vector3(noiseLevel, 0.1f, noiseLevel);
        sound.GetComponent<Renderer>().material.color = Color.red;
}

    void makeNoise(){
        Collider[] foundObjects = Physics.OverlapSphere(transform.position, noiseLevel/2);
        foreach(var currentObject in foundObjects)
        {
            if (currentObject.tag == "Enemy")
            {
                float distance = Vector3.Distance(transform.position, currentObject.transform.position);
                SoundSource source = new SoundSource(gameObject, noiseLevel / distance);
                Debug.Log("Noise: " + noiseLevel / distance + "For Object: " + currentObject.name);
                currentObject.transform.gameObject.SendMessage("NoiseReceived", source,
                    SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}

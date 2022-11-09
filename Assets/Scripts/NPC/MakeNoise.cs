using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeNoise : MonoBehaviour
{
    public float soundRadius = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //when space bar pressed, make noise
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("BOOOOM!!!!");   
            spawnSound();         
            makeNoise();
        }
    }

    void spawnSound(){
        GameObject sound = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        sound.transform.position = transform.position;
        sound.transform.Translate(0, -0.5f, 0);
        sound.transform.localScale = new Vector3(soundRadius, 0.1f, soundRadius);
        sound.GetComponent<Renderer>().material.color = Color.red;
}

    void makeNoise(){
        Collider[] foundObjects = Physics.OverlapSphere(transform.position, soundRadius/2);
        foreach(var currentObject in foundObjects)
        {
            currentObject.transform.gameObject.SendMessage("NoiseReceived", gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveNoise : MonoBehaviour
{
    public float speed = 1f;

    private bool alerted = false;
    private GameObject noiseMaker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move to source when alerted
        if (alerted)
        {
            transform.position = Vector3.MoveTowards(transform.position, noiseMaker.transform.position, speed * Time.deltaTime);
        }
    }

    void NoiseReceived(GameObject source)
    {
        alerted = true;
        noiseMaker = source;
    }
}

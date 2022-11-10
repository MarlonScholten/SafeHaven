using System.Collections;
using System.Collections.Generic;
using NPC;
using UnityEngine;

public class ReceiveNoise : MonoBehaviour
{
    public float speed = 1f;
    public float threshold = 0.1f;

    private bool alerted = false;
    private SoundSource noiseMaker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move to source when alerted
        if (alerted && noiseMaker.getVolume() > threshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, noiseMaker.getSource().transform.position, speed * Time.deltaTime);
        }
    }

    void NoiseReceived(SoundSource source)
    {
        alerted = true;
        noiseMaker = source;
    }
}

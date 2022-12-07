using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundTest : SoundBase
{
    // Start is called before the first frame update
    void Start()
    {
        playSound(gameObject);
        StartCoroutine(ExampleCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ExampleCoroutine()
    {
        Debug.Log("terst");
        yield return new WaitForSeconds(5);
        stopSound();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour, ITrigger
{
    public GameObject triggerObject {get;}
    Light light;

    void Start()
    {
        light = gameObject.GetComponent<Light>();
    }

    public void trigger()
    {
        light.enabled = !light.enabled;
    }
}
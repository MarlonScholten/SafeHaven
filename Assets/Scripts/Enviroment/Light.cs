using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour, ITrigger
{

    void Start()
    {
        
    }

    public void trigger()
    {
        //set object active
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
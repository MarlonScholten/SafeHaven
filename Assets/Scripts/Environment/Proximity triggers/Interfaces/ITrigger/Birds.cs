using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birds : MonoBehaviour, ITrigger
{
    private bool _triggered = false;

    public void trigger()
    {
        Debug.Log("Birds triggered");
        _triggered = true;
    }

    public void Update()
    {
        if (_triggered)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1 * Time.deltaTime, transform.position.z);
        }
    }
}

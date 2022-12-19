using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbineRotation : MonoBehaviour
{
    [SerializeField] private float _speed = 90.0f;
    
    void Update()
    {
        transform.rotation = Quaternion.Euler(
            transform.rotation.x,
            transform.rotation.eulerAngles.y + _speed * Time.deltaTime, 
            transform.rotation.z
        );
    }
}

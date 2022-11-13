using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] public float viewRadius = 20;
    [SerializeField] public float viewAngle = 60;

    public LayerMask targetMask;

    void FindClosestHidingSpot(){
        Collider[] objectsInView = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
    }

    // Start is called before the first frame update
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGLobal){
        if(!angleIsGLobal){
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

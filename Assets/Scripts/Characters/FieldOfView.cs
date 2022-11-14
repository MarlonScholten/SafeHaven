using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] public float viewRadius = 20;
    [SerializeField] public float viewAngle = 60;

    public LayerMask targetMask;

    public void FindClosestHidingSpot(){
        Collider[] objectsInView = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        if(objectsInView.Length != 0){
            Transform closestHidingSpot = objectsInView[0].transform;

            for(int i = 0; i<objectsInView.Length; i++){
                if(Vector3.Distance(transform.position,objectsInView[i].transform.position)<
                    Vector3.Distance(closestHidingSpot.position,transform.position)){
                        closestHidingSpot = objectsInView[i].transform;
                    }
            }
            Debug.Log(closestHidingSpot.position);
        }        
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

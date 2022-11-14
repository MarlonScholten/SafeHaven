using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] public float viewRadius = 80;
    [SerializeField] public float viewAngle = 60;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public Transform FindClosestHidingSpot(){
        Collider[] objectsInView = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        Transform closestHidingSpot = null;
        if(objectsInView.Length != 0){
            

            for(int i = 0; i<objectsInView.Length; i++){
                Transform obj = objectsInView[i].transform;
                Vector3 dirToObject = (objectsInView[i].transform.position - transform.position).normalized;
                if(Vector3.Angle(transform.forward, dirToObject) < viewAngle / 2){
                    float distanceToSpot = Vector3.Distance(transform.position,obj.position);
                    if(!Physics.Raycast(transform.position, dirToObject, distanceToSpot, obstacleMask)){
                        if(closestHidingSpot == null){
                            closestHidingSpot = objectsInView[i].transform;
                        }
                        else if(Vector3.Distance(closestHidingSpot.transform.position, transform.position)< distanceToSpot){
                                closestHidingSpot = objectsInView[i].transform;
                        }
                    }
                }
            }
        }
        if(closestHidingSpot == null){
            return null;
        }
        else{
            return closestHidingSpot;
        }        
    }

    // Start is called before the first frame update
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGLobal){
        if(!angleIsGLobal){
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

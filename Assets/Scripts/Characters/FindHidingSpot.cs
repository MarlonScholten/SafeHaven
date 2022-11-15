using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindHidingSpot : MonoBehaviour
{
    [SerializeField] public float _viewRadius = 80;
    [SerializeField] public float _viewAngle = 60;

    public LayerMask _targetMask;
    public LayerMask _obstacleMask;

    public Transform FindClosestHidingSpot(){
        Collider[] hidingSpots = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask);
        Transform closestHidingSpot = null;         
  
        foreach(Collider spot in hidingSpots){
            Transform hidingSpot = spot.transform;
            Vector3 dirToObject = (hidingSpot.position - transform.position).normalized;
                //Check if the spot is in the brothers view
            if(Vector3.Angle(transform.forward, dirToObject) < _viewAngle / 2){
                float distanceToSpot = Vector3.Distance(transform.position,hidingSpot.position);

                if(!Physics.Raycast(transform.position, dirToObject, distanceToSpot, _obstacleMask)){
                        //If there is not a current closest spot, then set one
                    if(closestHidingSpot == null){
                        closestHidingSpot = hidingSpot;
                    }
                        //Check if other location is closer than current closest location
                    else if(distanceToSpot < Vector3.Distance(closestHidingSpot.transform.position, transform.position)){
                            closestHidingSpot = hidingSpot;
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

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGLobal){
        if(!angleIsGLobal){
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

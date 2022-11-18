using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearSystem : MonoBehaviour
{
    [SerializeField]protected float _fear = 0f;
    private float _maxFearLevel = 1;
    [SerializeField]private float _comfortIncrement = 0.1f;
    [SerializeField]private float _fearIncement = 0.02f;

    [SerializeField]private float _maxFearDistance = 20f;

    [SerializeField]private float _viewRadius = 30f;
    [SerializeField]private float _viewAngle = 140f;
    [SerializeField] LayerMask _EnemyMask;

    void Update(){
        checkEnemyEncounter();

        if(Input.GetKeyDown(KeyCode.C)){
            Comfort();
        }
    }

    public void checkEnemyEncounter(){
        Collider[] enemies = Physics.OverlapSphere(transform.position, _viewRadius, _EnemyMask);
        foreach(Collider enemy in enemies){
            Transform enemyPos = enemy.transform;
            Vector3 dirToEnemy = (enemyPos.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, enemyPos.position);
            if(Vector3.Angle(transform.forward, dirToEnemy) < _viewAngle / 2){
                if(Physics.Raycast(transform.position, dirToEnemy, out RaycastHit hit, distance)){
                    if(hit.collider.tag == "Enemy"){
                            UpdateFearLevel(distance);
                    }
                }                    
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGLobal){
        if(!angleIsGLobal){
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

            // foreach(Collider spot in hidingSpots){
            // Transform hidingSpot = spot.transform;
            // Vector3 dirToObject = (hidingSpot.position - transform.position).normalized;
            //     //Check if the spot is in the brothers view
            // if(Vector3.Angle(transform.forward, dirToObject) < _viewAngle / 2){
            //     float distanceToSpot = Vector3.Distance(transform.position,hidingSpot.position);

            //     if(!Physics.Raycast(transform.position, dirToObject, distanceToSpot, _obstacleMask)){
            //             //If there is not a current closest spot, then set one
            //         if(closestHidingSpot == null){
            //             closestHidingSpot = hidingSpot;
            //         }
            //             //Check if other location is closer than current closest location
            //         else if(distanceToSpot < Vector3.Distance(closestHidingSpot.transform.position, transform.position)){
            //                 closestHidingSpot = hidingSpot;
            //         }
            //     }

    public void UpdateFearLevel(float distance){
        if(distance < _maxFearDistance && _fear < _maxFearLevel){
            _fear += (_fearIncement - (distance / 1000f)) * Time.deltaTime;
        }
    }

    public void Comfort(){
        if(_fear > 0){
            _fear -= _comfortIncrement;
        }
        if(_fear < _comfortIncrement){
            _fear = 0;
        }
    }
}

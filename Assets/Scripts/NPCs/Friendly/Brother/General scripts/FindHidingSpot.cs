using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FindHidingSpot : MonoBehaviour
{
    [SerializeField] private float _hidingSpotInEnemyViewWeight = 0.01f;
    [SerializeField] private float _hidingSpotDistanceWeight = 0.05f;

    [SerializeField] private Camera _playerCamera; 
    public Transform FindBestHidingSpot(){
        GameObject[] _hidingSpots = GameObject.FindGameObjectsWithTag("hideable");
        GameObject bestSpot = null;
        float bestSpotValue = -1;
        foreach(GameObject spot in _hidingSpots){
            if(CheckHidingSpotInView(spot)){
                HidingSpot hidingSpot = spot.GetComponent<HidingSpot>();
                CalculateObscurityValue(spot);           
                if((bestSpotValue == -1) || (bestSpotValue < hidingSpot._obscurityValue)){
                    bestSpot = spot;
                    bestSpotValue = hidingSpot._obscurityValue;
                }
            }
        }
        return bestSpot.transform;    
    }

    private bool CheckHidingSpotInView(GameObject hidingSpot){
        Vector3 viewport = _playerCamera.WorldToViewportPoint(hidingSpot.transform.position);
         bool inCameraFrustum = IsBetween0And1(viewport.x) && IsBetween0And1(viewport.y);
         bool inFrontOfCamera = viewport.z > 0;
 
         RaycastHit depthCheck;
         bool objectBlockingPoint = false;
 
         Vector3 directionBetween = hidingSpot.transform.position - _playerCamera.transform.position;
         directionBetween = directionBetween.normalized;
 
         float distance = Vector3.Distance(_playerCamera.transform.position, hidingSpot.transform.position);
 
         if(Physics.Raycast(_playerCamera.transform.position, directionBetween, out depthCheck, distance + 0.05f)) {
             if(depthCheck.point != hidingSpot.transform.position) {
                 objectBlockingPoint = true;
             }
         }
 
         return inCameraFrustum && inFrontOfCamera && !objectBlockingPoint;
    }

    private bool IsBetween0And1(float a) {
         return (a > 0) && (a < 1);
    }

    private void CalculateObscurityValue(GameObject spot){
        HidingSpot hidingSpot = spot.GetComponent<HidingSpot>();
        hidingSpot._obscurityValue = hidingSpot._grade - (Vector3.Distance(hidingSpot.transform.position, transform.position) * _hidingSpotDistanceWeight);
        if(CheckIfSpotInEnemyFov()){
            hidingSpot._obscurityValue *= _hidingSpotInEnemyViewWeight;
        }
    }



    private bool CheckIfSpotInEnemyFov(){
        //Will need to implement method in Enemy AI to do a raycast to the position and check if there are any objects in between.
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FindHidingSpot : MonoBehaviour
{
    [SerializeField] private float _HidingSpotInEnemyViewWeight = 0.01f;
    [SerializeField] private float _HidingSpotDistanceWeight = 0.05f;
    public Transform FindBestHidingSpot(){
        GameObject[] _hidingSpots = GameObject.FindGameObjectsWithTag("hideable");
        GameObject bestSpot = null;
        float bestSpotValue = -1;
        foreach(GameObject spot in _hidingSpots){
            HidingSpot hidingSpot = spot.GetComponent<HidingSpot>();
            CalculateObscurityValue(spot);           
            if(bestSpotValue == -1){
                bestSpot = spot;
                bestSpotValue = hidingSpot._obscurityValue;
            }
            else if(bestSpotValue < hidingSpot._obscurityValue){
                bestSpot = spot;
            }
        }     
        return bestSpot.transform;    
    }

    private void CheckHidingSpotInView(){
        
    }

    private void CalculateObscurityValue(GameObject spot){
        HidingSpot hidingSpot = spot.GetComponent<HidingSpot>();
        hidingSpot._obscurityValue = hidingSpot._grade - (Vector3.Distance(hidingSpot.transform.position, transform.position) * _HidingSpotDistanceWeight);
        if(CheckIfSpotInEnemyFov()){
            hidingSpot._obscurityValue *= _HidingSpotInEnemyViewWeight;
        }
    }

    private bool CheckIfSpotInEnemyFov(){
        //Will need to implement method in Enemy AI to do a raycast to the position and check if there are any objects in between.
        return false;
    }
}

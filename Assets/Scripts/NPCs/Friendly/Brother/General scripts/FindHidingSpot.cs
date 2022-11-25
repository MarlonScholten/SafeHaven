using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
/// <summary>
/// Author: Jelco van der Straaten </para>
/// Modified by:  Jelco</para>
/// This script calculates the best hidingspots
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>EmptyGameObject</term>
///		    <term>Tag</term>
///         <term>hideable</term>
///		    <term>The tag is needed to specify this empty gameObject references a hiding spot.</term>
///	    </item>
///     <item>
///         <term>Enemy</term>
///		    <term>Tag</term>
///         <term>Enemy</term>
///		    <term>The tag is needed to lower the value of the hidingspot if the hidingspot is visible to the enemy.</term>
///	    </item>
/// </list>
public class FindHidingSpot : MonoBehaviour
{
    [SerializeField, Range(0.0f, 0.1f), Tooltip("This value determines by how much the hiding spot gets multiplied if it is in an enemy view")] 
    private float _hidingSpotInEnemyViewWeight = 0.01f;

    [SerializeField, Range(0.0f, 0.2f), Tooltip("This value determines the weight of the distance of the hiding spot.")] 
    private float _hidingSpotDistanceWeight = 0.05f;

    [SerializeField , Tooltip("Please put the playerCamera in this field.")] private Camera _playerCamera;

    /// <summary>
    /// This script gahters all hiding spots and then calculates depending on the grade, distance and visibility to the enemy the best hiding spot for the brother.
    /// </summary> 
    public Vector3 FindBestHidingSpot(){
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
        if(bestSpot != null){
        return bestSpot.transform.position;
        }
        else {
            CustomEvent.Trigger(this.gameObject, "Follow");
            return new Vector3();
        }    
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
        if(CheckIfSpotInEnemyFov(spot)){
            hidingSpot._obscurityValue *= _hidingSpotInEnemyViewWeight;
        }
    }



    private bool CheckIfSpotInEnemyFov(GameObject spot){
        bool isVisible = false;
        // Will need to implement method in Enemy AI to do a raycast to the position and check if there are any objects in between.
        // Maybe could implement it with enemy FOV in mind but for now this is an temporary solution, beceause enemy needs to implement this.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies){
            Transform enemyPos = enemy.transform;
            Vector3 dirToEnemy = (enemyPos.position - spot.transform.position).normalized;
            float distance = Vector3.Distance(spot.transform.position, enemyPos.position);
            if(Physics.Raycast(spot.transform.position, dirToEnemy, out RaycastHit hit, distance + 0.1f)){
                if(hit.collider.tag == "Enemy"){
                        isVisible = true;
                }                                      
            }
        }
        return isVisible;
    }
}

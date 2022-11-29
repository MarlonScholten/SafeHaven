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
        GameObject[] hidingSpots = GameObject.FindGameObjectsWithTag("hideable");
        GameObject bestSpot = null;
        float bestSpotValue = -1;
        foreach(var spot in hidingSpots)
        {
            if (!CheckHidingSpotInView(spot)) continue;
            // Now we calculate the obscurity value for the current hiding spot to be checked.
            HidingSpot hidingSpot = spot.GetComponent<HidingSpot>();
            CalculateObscurityValue(spot);
            // If there isn't currently not an bestpot this spot is automaticly the new best spot.
            // If there is an best spot it checks if the value is higher than the current best spot value.           
            if((bestSpotValue.Equals(-1)) || (bestSpotValue < hidingSpot._obscurityValue)){
                bestSpot = spot;
                bestSpotValue = hidingSpot._obscurityValue;
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

    /// <summary>
    /// This function checks if the hidingspot is in the view of the camera. If so it returns true else it returns false.
    /// </summary>
    /// <param name="hidingSpot">The hiding spot to be checked if it is in the view.</param>
    /// <returns>True for hidingspots in camera view, false if not in camera view</returns>
    private bool CheckHidingSpotInView(GameObject hidingSpot){
        var hidingSpotPosition = hidingSpot.transform.position;
        Vector3 viewport = _playerCamera.WorldToViewportPoint(hidingSpotPosition);
         bool inCameraFrustum = IsBetween0And1(viewport.x) && IsBetween0And1(viewport.y);
         bool inFrontOfCamera = viewport.z > 0;
 
         RaycastHit depthCheck;
         bool objectBlockingPoint = false;

         var cameraTransform = _playerCamera.transform;
         Vector3 directionBetween = hidingSpotPosition - cameraTransform.position;
         directionBetween = directionBetween.normalized;
 
         float distance = Vector3.Distance(cameraTransform.position, hidingSpotPosition);
 
         if(Physics.Raycast(_playerCamera.transform.position, directionBetween, out depthCheck, distance + 0.05f)) {
             if(depthCheck.point != hidingSpot.transform.position) {
                 objectBlockingPoint = true;
             }
         }
 
         return inCameraFrustum && inFrontOfCamera && !objectBlockingPoint;
    }

    /// <summary>
    /// This function simply checks if an value is between 0 and 1.
    /// </summary>
    /// <param name="a">Value to be checked if it is between 0 and 1</param>
    /// <returns>True for values between 0 and 1</returns>
    private bool IsBetween0And1(float a) {
         return (a > 0) && (a < 1);
    }

    /// <summary>
    /// This function calculates the ObscurityValue for the hiding spot.
    /// First it gets the grade and than subtracts the distance to the spot of the grade.
    /// Then checks if the spot is in the enemy view, if so the grade gets lowered, else nothing happens.
    /// </summary>
    /// <param name="spot">Hiding spot where the value needs to be calculated for.</param>
    private void CalculateObscurityValue(GameObject spot){
        HidingSpot hidingSpot = spot.GetComponent<HidingSpot>();
        hidingSpot._obscurityValue = hidingSpot._grade - (Vector3.Distance(hidingSpot.transform.position, transform.position) * _hidingSpotDistanceWeight);
        if(CheckIfSpotInEnemyFov(spot)){
            hidingSpot._obscurityValue *= _hidingSpotInEnemyViewWeight;
        }
    }



    /// <summary>
    /// This function checks if the hiding spot is in the enemy fov.
    /// </summary>
    /// <param name="spot">Hiding spot which will be checked if can be seen by enemy.</param>
    /// <returns>True for values if enemy is in sight of hiding Spot</returns>
    private bool CheckIfSpotInEnemyFov(GameObject spot){
        bool isVisible = false;
        // TODO: Will need to implement method in Enemy AI to do a raycast to the position and check if there are any objects in between.
        // Maybe could implement it with enemy FOV in mind but for now this is an temporary solution, beceause enemy needs to implement this.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies){
            Transform enemyPos = enemy.transform;
            var hidingSpotPosition = spot.transform.position;
            var enemyPosition = enemyPos.position;
            Vector3 dirToEnemy = (enemyPosition - hidingSpotPosition).normalized;
            float distance = Vector3.Distance(hidingSpotPosition, enemyPosition);
            if(Physics.Raycast(spot.transform.position, dirToEnemy, out RaycastHit hit, distance + 0.1f)){
                if(hit.collider.CompareTag("Enemy")){
                        isVisible = true;
                }                                      
            }
        }
        return isVisible;
    }
}

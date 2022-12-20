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
    

    /// <summary>
    /// This script gahters all hiding spots and then calculates depending on the grade, distance and visibility to the enemy the best hiding spot for the brother.
    /// </summary> 
    public Vector3 FindBestHidingSpot(Vector3 playerPos, float range){
        GameObject[] hidingSpots = GameObject.FindGameObjectsWithTag("hideable");
        GameObject bestSpot = null;
        float bestSpotValue = -1;
        foreach(var spot in hidingSpots)
        {
            if (Vector3.Distance(playerPos, spot.transform.position) > range - 4f) continue;
            // Now we calculate the obscurity value for the current hiding spot to be checked.
            HidingSpot hidingSpot = spot.GetComponent<HidingSpot>();
            CalculateObscurityValue(spot);
            // If there isn't currently not an bestpot this spot is automaticaly the new best spot.
            // If there is an best spot it checks if the value is higher than the current best spot value.           
            if ((!bestSpotValue.Equals(-1)) && (!(bestSpotValue < hidingSpot._obscurityValue))) continue;
            bestSpot = spot;
            bestSpotValue = hidingSpot._obscurityValue;
        }

        // If a spot was found return the position of the hiding spot.
        return bestSpot != null ? bestSpot.transform.position :
            // If there wasn't a hiding spot near, return empty vector.
            new Vector3();
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
        foreach(var enemy in enemies){
            
            var hidingSpotPosition = spot.transform.position;
            var enemyPosition = enemy.transform.position;
            Vector3 dirToEnemy = (enemyPosition - hidingSpotPosition).normalized;
            float distance = Vector3.Distance(hidingSpotPosition, enemyPosition);

            if (!Physics.Raycast(spot.transform.position, dirToEnemy, out RaycastHit hit, distance + 0.1f)) continue;
            if(hit.collider.CompareTag("Enemy")){
                isVisible = true;
            }
        }
        return isVisible;
    }
}

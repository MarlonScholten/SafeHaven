using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Jelco van der Straaten </para>
/// Modified by:  Jelco</para>
/// This script controls the fear of the broter.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>Enemy</term>
///		    <term>Tag</term>
///         <term>Enemy</term>
///		    <term>The tag is needed to determine if a object is an enemy, so that the fear can be affected.</term>
///	    </item>
/// </list>

public class FearSystem : MonoBehaviour
{
    /// <summary>
    /// This value determines the current fear value of the brother. When its 0 the brother is comfortable. And when its 1 he is fully feared. 
    /// </summary>
    [SerializeField, Range(0.0f, 100.0f), Tooltip("This value determines the current fear value of the brother.")]
    private float _fear = 0f;

    private readonly float _maxFearLevel = 100;


    [SerializeField, Range(5.0f, 15.0f), Tooltip("This value determines how much the brothers fear lowers when comforted by the sister.")]
    private float _comfortIncrement = 10f;


    [SerializeField, Range(0.0f, 15.0f), Tooltip("This value determines how fast the brother gets scared.")]
    private float _fearIncement = 8f;

    [SerializeField, Range(10.0f, 50.0f), Tooltip("This value determines the maximum distance that an enemy interaction will affect the brothers fear.")]
    private float _maxFearDistance = 20f;

    [SerializeField, Range(10.0f, 50.0f), Tooltip("This value determines how far the brother can see.")]
    private float _viewRadius = 30f;

    [SerializeField, Range(60.0f, 180.0f), Tooltip("This value determines the angle of view of the brother.")]
    private float _viewAngle = 140f;

    [SerializeField, Tooltip("This value determines the layer the brother is in.")]
    private LayerMask _EnemyMask;

    void FixedUpdate(){
        checkEnemyEncounter();
    }

    /// <summary>
    /// This function checks if the brother can see an enemy(encounter).
    /// </summary>
    public void checkEnemyEncounter(){
        Collider[] enemies = Physics.OverlapSphere(transform.position, _viewRadius, _EnemyMask);
        foreach(Collider enemy in enemies){
            Transform enemyPos = enemy.transform;
            Vector3 dirToEnemy = (enemyPos.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, enemyPos.position);
            if((Vector3.Angle(transform.forward, dirToEnemy)) < (_viewAngle / 2)){
                if(Physics.Raycast(transform.position, dirToEnemy, out RaycastHit hit, distance)){
                    if(hit.collider.tag == "Enemy"){
                            UpdateFearLevel(distance);
                    }
                }                    
            }
        }
    }

    /// <summary>
    /// This function updates the fear level. The function gets called when the brother encounters an enemy.
    /// </summary>
    public void UpdateFearLevel(float distance){
        if((distance < _maxFearDistance) && (_fear < _maxFearLevel)){
            //Time.deltatime is needed beceause fear gets updated every frame.
            _fear += (_fearIncement - (distance /10f)) * Time.deltaTime;
        }
    }

    /// <summary>
    /// This function decreases the fear level. It gets called when the player comforts the brother. </para>
    /// The brother then gets comforted by the amount set bij the comfortIncrement.
    /// </summary>
    public void Comfort(){
        if(_fear > 0){
            _fear -= _comfortIncrement;
        }
        if(_fear < _comfortIncrement){
            _fear = 0;
        }
    }
}

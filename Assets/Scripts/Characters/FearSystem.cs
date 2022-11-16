using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearSystem : MonoBehaviour
{
    protected float _fear = 0f;
    private float _maxFearLevel = 1;
    private float _comfort = 0.1f;
    private float _fearIncement = 0.02f;

    private float _maxFearDistance = 20f;

    private float _viewRadius = 30f;
    [SerializeField] LayerMask _EnemyMask;

    void Update(){
        checkEnemyEncounter();
    }

    public void checkEnemyEncounter(){
        Collider[] enemies = Physics.OverlapSphere(transform.position, _viewRadius, _EnemyMask);
        foreach(Collider enemy in enemies){
            Transform enemyPos = enemy.transform;
            Vector3 dirToEnemy = (enemyPos.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, enemyPos.position);
            if(Physics.Raycast(transform.position, dirToEnemy, out RaycastHit hit, distance)){
                if(hit.collider.tag == "Enemy"){
                        UpdateFearLevel(distance);
                }                    
            }
        }
    }

    public void UpdateFearLevel(float distance){
        if(distance < _maxFearDistance && _fear < _maxFearLevel){
            _fear += (_fearIncement - (distance / 1000f)) * Time.deltaTime;
        }
    }

    public void Comfort(){
        if(_fear > 0){
            _fear -= _comfort;
        }
    }
}

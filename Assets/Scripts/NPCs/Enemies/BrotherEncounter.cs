using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrotherEncounter : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other){
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Brother"){
            var fearSystem = other.GetComponentInParent<FearSystem>();
            fearSystem.checkEnemyEncounter(this.gameObject);
        }
    }
}

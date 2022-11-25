using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Jelco van der Straaten </para>
/// Modified by:  Jelco</para>
/// This script detects that the brother nears an enemy
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>Brother</term>
///		    <term>Tag</term>
///         <term>Brother</term>
///		    <term>The tag is needed so that the script can use the correct script on the correct object.</term>
///	    </item>
///     <item>
///         <term>Brother</term>
///		    <term>Tag</term>
///         <term>Brother</term>
///		    <term>The tag is needed so that the script can use the correct script on the correct object.</term>
///	    </item>
/// </list>
public class BrotherEncounter : MonoBehaviour
{
    private void OnTriggerStay(Collider other){
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Brother"){
            var fearSystem = other.GetComponentInParent<FearSystem>();
            fearSystem.checkEnemyEncounter(this.gameObject);
        }
    }
}

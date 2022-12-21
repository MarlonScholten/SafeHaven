using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Tom Cornelissen<br/>
/// Modified by:  <br/>
/// Description: Plays a voiceline when the player enters a trigger
/// /// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>VoicelineTrigger</term>
///         <term>Script</term>
///         <term>This Script</term>
///         <term>This lives on the prefabs of VoicelineTrigger</term>
///     </item>
/// </list>
public class VoicelineTrigger : MonoBehaviour
{
    [SerializeField]
    [Tooltip("A reference to the wwise voiceline sequence")]
    private AK.Wwise.Event _voicelineSequence;
    
    [SerializeField]
    [Tooltip("The amount of voicelines in the sequence")]
    private int _sequenceLength;

    [SerializeField] 
    [Tooltip("The amount of time between each voiceline")]
    private List<float> _voicelineInterval;

    [SerializeField]
    [Tooltip("The GameObject that each voiceline is coming from")]
    private List<GameObject> _voicelineSource;

    private bool _activated = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_activated) return;

        _activated = true;

        StartCoroutine(PlayVoicelines());
    }

    private IEnumerator PlayVoicelines()
    {
        for (int i = 0; i < _sequenceLength; i++)
        {
            _voicelineSequence.Post(_voicelineSource[i]);

            if (i >= _sequenceLength - 1) break;
            yield return new WaitForSeconds(_voicelineInterval[i]);
        }
    }
}

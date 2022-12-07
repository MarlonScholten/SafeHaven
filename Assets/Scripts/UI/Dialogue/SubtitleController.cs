using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Author: Hugo Ulfman </para>
/// Modified by: N/A </para>
/// This script contains the functions to start subtitles from certain scenes.
/// This script can also empty the subtitles when a voiceline is done.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>Should be placed on the Dialogue controller</term>
///		    <term>Script</term>
///         <term>SubtitleController</term>
///		    <term>This script contains the functions to start subtitles from certain scenes. This script can also empty the subtitles when a voiceline is done.</term>
///	    </item>
/// </list>
public class SubtitleController : MonoBehaviour
{
    /// <summary>
    /// The SubtitleScriptableObject that contains the subtitles.
    /// </summary>
    [SerializeField]
    private SubtitleScriptableObject subs;

    /// <summary>
    /// The text that will be changed to show the subtitles.
    /// </summary>
    [SerializeField]
    private TMP_Text text;
    
    /// <summary>
    /// The function that will be called when the scene starts.
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// This function can show a certain subtitle from a certain scene.
    /// </summary>
    /// <param name="sceneNr">The scene number of the subtitle.</param>
    /// <param name="voiceLineNr">The voice line number of the subtitle.</param>
    void showSubtitle(int sceneNr, int voiceLineNr) {
        //loop through subtitles
        foreach (SubtitleScriptableObject.SubtitleList subtitleList in subs.subs) {
            if (subtitleList.sceneNr == sceneNr) {
                foreach (SubtitleScriptableObject.Subtitle subtitle in subtitleList.subtitles) {
                    if (subtitle.voiceLineNr == voiceLineNr) {
                        text.text = subtitle.character + ": " + subtitle.text;
                    }
                }
            }
        }
    }

    /// <summary>
    /// This function can empty the subtitles.
    /// </summary>
    void emptySubtitle() {
        text.text = "";
    }
}

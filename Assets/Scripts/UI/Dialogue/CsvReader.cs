using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Author: Hugo Ulfman </para>
/// Modified by: N/A </para>
/// This script reads a given csv file and adds the subtitles to the SubtitleScriptableObject.
/// There is a CsvReader editor script that makes it possible to read the csv file from the editor.
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
///         <term>CsvReader</term>
///		    <term>This script reads a given csv file and adds the subtitles to the SubtitleScriptableObject.</term>
///	    </item>
/// </list>
public class CsvReader : MonoBehaviour
{
    /// <summary>
    /// The SubtitleScriptableObject that will be filled with the subtitles from the csv file.
    /// </summary>
    [SerializeField]
    private SubtitleScriptableObject subs;

    /// <summary>
    /// The csv file that will be read.
    /// </summary>
    [SerializeField]
    private TextAsset csvFile;


    /// <summary>
    /// Reads the csv file and adds the subtitles to the SubtitleScriptableObject.
    /// </summary>
    public void AddSubtitlesWithCSV()
    {
        int currentScene = 1;
        //split csv file into lines
        string[] lines = csvFile.text.Split("\n"[0]);
        //create a list of subtitles
        List<SubtitleScriptableObject.Subtitle> subtitles = new List<SubtitleScriptableObject.Subtitle>();

        //loop through lines
        for (int i = 1; i < lines.Length; i++)
        {
            //split line into columns
            string[] columns = lines[i].Split(";");
            if (currentScene != int.Parse(columns[0])) {
                //create a new subtitle list
                SubtitleScriptableObject.SubtitleList subtitleList = new SubtitleScriptableObject.SubtitleList();

                //set subtitle list properties
                subtitleList.sceneNr = currentScene;
                subtitleList.subtitles = subtitles;

                //add subtitle list to scriptable object
                if (checkIfSceneExists(currentScene)) {
                    replaceScene(currentScene, subtitleList);
                }
                else {
                    subs.subs.Add(subtitleList);
                }
                currentScene = int.Parse(columns[0]);
                subtitles = new List<SubtitleScriptableObject.Subtitle>();
            }

            //create a new subtitle
            SubtitleScriptableObject.Subtitle subtitle = new SubtitleScriptableObject.Subtitle();

            //set subtitle properties
            subtitle.voiceLineNr = int.Parse(columns[1]);
            subtitle.character = columns[2];
            subtitle.text = columns[3];
            subtitle.time = int.Parse(columns[4]);

            //add subtitle to list
            subtitles.Add(subtitle);
        }
        //create a new subtitle list
        SubtitleScriptableObject.SubtitleList subtitleListFinal = new SubtitleScriptableObject.SubtitleList();

        //set subtitle list properties
        subtitleListFinal.sceneNr = currentScene;
        subtitleListFinal.subtitles = subtitles;

        //add subtitle list to scriptable object
        if (checkIfSceneExists(currentScene)) {
            replaceScene(currentScene, subtitleListFinal);
        }
        else {
            subs.subs.Add(subtitleListFinal);
        }
        
    }

    /// <summary>
    /// Checks if the scene already exists in the SubtitleScriptableObject.
    /// </summary>
    /// <param name="sceneNr">The scene number to check.</param>
    /// <returns>True if the scene exists, false if it doesn't.</returns>
    bool checkIfSceneExists(int sceneNr) {
        foreach (SubtitleScriptableObject.SubtitleList subtitleList in subs.subs) {
            if (subtitleList.sceneNr == sceneNr) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Replaces the subtitles of the given scene with the given subtitles.
    /// </summary>
    /// <param name="sceneNr">The scene number to replace.</param>
    /// <param name="subtitleList">The subtitles to replace with.</param>
    void replaceScene(int sceneNr, SubtitleScriptableObject.SubtitleList subtitleList) {
        for (int i = 0; i < subs.subs.Count; i++) {
            if (subs.subs[i].sceneNr == sceneNr) {
                subs.subs[i] = subtitleList;
            }
        }
    }
}


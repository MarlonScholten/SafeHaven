using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SubtitleExtractor : MonoBehaviour
{
    [MenuItem("Tools/Subtitles/Extract Subtitles")]
    public static void ShowWindow()
    {
        AddSubtitlesWithCSV();
    }

    /// <summary>
    /// Reads the csv file and adds the subtitles to the SubtitleScriptableObject.
    /// </summary>
    public static void AddSubtitlesWithCSV()
    {
        SubtitleScriptableObject subs = (SubtitleScriptableObject)AssetDatabase.LoadAssetAtPath("Assets/Scripts/UI/Dialogue/SubtitleScriptableObject.asset", typeof(SubtitleScriptableObject));
        var csvFile = File.ReadAllText("Assets/Scripts/UI/Dialogue/subtitles.csv");
        
        int currentScene = 1;
        //split csv file into lines
        string[] lines = csvFile.Split("\n"[0]);
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
                if (checkIfSceneExists(currentScene, subs)) {
                    replaceScene(currentScene, subtitleList, subs);
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
        if (checkIfSceneExists(currentScene, subs)) {
            replaceScene(currentScene, subtitleListFinal, subs);
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
    static bool checkIfSceneExists(int sceneNr, SubtitleScriptableObject _subs) {
        foreach (SubtitleScriptableObject.SubtitleList subtitleList in _subs.subs) {
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
    static void replaceScene(int sceneNr, SubtitleScriptableObject.SubtitleList subtitleList, SubtitleScriptableObject _subs) {
        for (int i = 0; i < _subs.subs.Count; i++) {
            if (_subs.subs[i].sceneNr == sceneNr) {
                _subs.subs[i] = subtitleList;
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsvReader : MonoBehaviour
{
    [SerializeField]
    private SubtitleScriptableObject subs;

    [SerializeField]
    private TextAsset csvFile;


    //create function to add subtitles with csv file
    public void AddSubtitlesWithCSV()
    {
        int currentScene = 0;
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

    bool checkIfSceneExists(int sceneNr) {
        foreach (SubtitleScriptableObject.SubtitleList subtitleList in subs.subs) {
            if (subtitleList.sceneNr == sceneNr) {
                return true;
            }
        }
        return false;
    }

    void replaceScene(int sceneNr, SubtitleScriptableObject.SubtitleList subtitleList) {
        for (int i = 0; i < subs.subs.Count; i++) {
            if (subs.subs[i].sceneNr == sceneNr) {
                subs.subs[i] = subtitleList;
            }
        }
    }
    
    void Awake()
    {
        //subs.subs.Clear();

        //add subtitles with csv
        AddSubtitlesWithCSV();
    }
}


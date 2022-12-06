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
        SubtitleScriptableObject.SubtitleList subtitleList = new SubtitleScriptableObject.SubtitleList();
        //if the list is empty for the current scene, add a new list
        if (subs.subs.Count != 0) {
            if (subs.subs[currentScene] != null)
            {
                subtitleList = subs.subs[currentScene];
            }
        }


        //create a string array with the lines of the csv file
        string[] lines = csvFile.text.Split('\n');

        //create a for loop to go through the lines
        for (int i = 1; i < lines.Length; i++)
        {
            subtitleList.sceneNr = currentScene;
            //create a string array with the values of the line
            string[] values = lines[i].Split(';');
            Debug.Log(values[0]);
            Debug.Log(values[1]);
            Debug.Log(values[2]);
            Debug.Log(values[3]);
            if (int.Parse(values[0]) == currentScene) {
                //create a new subtitle
                SubtitleScriptableObject.Subtitle subtitle = new SubtitleScriptableObject.Subtitle();

                //set the values of the subtitle
                subtitle.character = values[1];
                subtitle.text = values[2];
                subtitle.time = int.Parse(values[3]);

                //add the subtitle to the list if it does not exist yet
                if (!subtitleList.subtitles.Contains(subtitle))
                {
                    subtitleList.subtitles.Add(subtitle);
                }
            }
            else {
                subs.subs[currentScene] = subtitleList;
                //create a new subtitle list
                subtitleList = new SubtitleScriptableObject.SubtitleList();
                subtitleList.sceneNr = currentScene;

                currentScene = int.Parse(values[0]);

                //create a new subtitle
                SubtitleScriptableObject.Subtitle subtitle = new SubtitleScriptableObject.Subtitle();

                //set the values of the subtitle
                subtitle.character = values[1];
                subtitle.text = values[2];
                subtitle.time = int.Parse(values[3]);

                //add the subtitle to the list if it does not exist yet
                if (!subs.subs[currentScene].subtitles.Contains(subtitle))
                {
                    subs.subs[currentScene].subtitles.Add(subtitle);
                }
            }

            
        }
    }
    
    
    private void Awake()
    {
        //add subtitles with csv
        AddSubtitlesWithCSV();
    }

    private void OnApplicationQuit()
    {
        //empty scriptable object
        subs.subs.Clear();
    }


}

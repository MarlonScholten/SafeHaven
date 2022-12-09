using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Author: Hugo Ulfman </para>
/// Modified by: N/A </para>
/// This script adds a button to the inspector of the dialogue controller to make it possible to read the csv file without starting the game.
/// </summary>

[CustomEditor(typeof(CsvReader))]
[CanEditMultipleObjects]
public class CsvReaderEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CsvReader myScript = (CsvReader)target;
        if(GUILayout.Button("Read CSV"))
        {
            myScript.AddSubtitlesWithCSV();
        }
    }
}

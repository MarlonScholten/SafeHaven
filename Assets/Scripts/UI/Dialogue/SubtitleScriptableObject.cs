using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Hugo Ulfman </para>
/// Modified by: N/A </para>
/// This script is a scriptable object to contain the subtitles.
/// </summary>
[CreateAssetMenu(fileName = "SubtitleScriptableObject", menuName = "Subtitles", order = 1)]
public class SubtitleScriptableObject : ScriptableObject
{
    /// <summary>
    /// A class to contain a subtitle.
    /// </summary>
    [System.Serializable]
    public class Subtitle
    {
        public int voiceLineNr;
        public string character;
        public string text;
        public int time;
    }

    /// <summary>
    /// A class to contain a list of subtitles.
    /// </summary>
    [System.Serializable]
    public class SubtitleList
    {
        public int sceneNr;
        public List<Subtitle> subtitles = new List<Subtitle>();
    }

    /// <summary>
    /// A list of subtitle lists.
    /// </summary>
    public List<SubtitleList> subs = new List<SubtitleList>();
    

}
        

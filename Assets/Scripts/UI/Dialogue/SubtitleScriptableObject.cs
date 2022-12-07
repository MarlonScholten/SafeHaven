using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SubtitleScriptableObject", menuName = "Subtitles", order = 1)]
public class SubtitleScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class Subtitle
    {
        public int voiceLineNr;
        public string character;
        public string text;
        public int time;
    }

    [System.Serializable]
    public class SubtitleList
    {
        public int sceneNr;
        public List<Subtitle> subtitles = new List<Subtitle>();
    }

    //create a list of subtitles
    public List<SubtitleList> subs = new List<SubtitleList>();
    

}
        

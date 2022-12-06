using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSubtitles : MonoBehaviour
{
    [SerializeField]
    private SubtitleScriptableObject subs;

    
    // Start is called before the first frame update
    void Start()
    {
        //print the subtitles
        foreach (SubtitleScriptableObject.SubtitleList subtitle in subs.subs)
        {
            foreach (SubtitleScriptableObject.Subtitle sub in subtitle.subtitles)
            {
                Debug.Log(sub.character + ": " + sub.text);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

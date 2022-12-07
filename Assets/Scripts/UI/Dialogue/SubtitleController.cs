using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleController : MonoBehaviour
{
    [SerializeField]
    private SubtitleScriptableObject subs;

    //ui text mesh pro
    [SerializeField]
    private TMP_Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        showSubtitle(0,1);
    }

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

    void emptySubtitle() {
        text.text = "";
    }
}

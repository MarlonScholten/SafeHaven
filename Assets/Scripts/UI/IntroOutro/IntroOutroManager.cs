using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroOutroManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject _introScreen;
    [SerializeField]
    private GameObject _outroScreen;

    [Header("Settings")]
    [SerializeField]
    private bool _introOnStartup = true;

    private void Start()
    {
        if (_introOnStartup)
            StartIntro();
    }

    public void StartIntro() => Instantiate(_introScreen);

    public void StartOutro(float alpha = 185)
    {
        OutroScreen outro = Instantiate(_outroScreen).GetComponent<OutroScreen>();
        outro.SetAlpha(185);
    }
}

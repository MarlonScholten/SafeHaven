using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroOutroManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _introScreen;
    [SerializeField]
    private GameObject _outroScreen;

    private void Start()
    {
        StartIntro();
    }

    public void StartIntro() => Instantiate(_introScreen);

    public void StartOutro() => Instantiate(_outroScreen);
}

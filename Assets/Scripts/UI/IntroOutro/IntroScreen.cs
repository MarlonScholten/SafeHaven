using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : UIScreen
{
    [Header("References")]
    [SerializeField]
    [Tooltip("The prefab that contains the continue button.")]
    private Button _continue;

    protected override void Awake()
    {
        base.Awake();
        _continue.onClick.AddListener(OnContinue);
    }

    private void Start()
    {
        Pause();
        _continue.Select();
    }

    private void OnContinue()
    {
        Resume();
        Destroy(gameObject);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : UIScreen
{
    [SerializeField]
    private Button _continue;

    protected override void Awake()
    {
        base.Awake();

        _continue.onClick.AddListener(OnContinue);
    }

    private void Start()
    {
        Pause();
    }

    private void OnContinue()
    {
        Resume();
        Destroy(gameObject);
    }
}

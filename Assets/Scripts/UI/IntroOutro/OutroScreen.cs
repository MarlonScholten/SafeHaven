using System;
using System.Collections;
using UnityEngine;

public class OutroScreen : UIScreen
{
    [SerializeField]
    [Range(1, 100)]
    private float _movementSpeed;

    [SerializeField]
    private RectTransform _container;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Pause();
        StartCoroutine(CreditCoroutine());
    }

    private IEnumerator CreditCoroutine()
    {
        // Remaps the clamped 0 to 100 range to 200 to 10.
        float remapped = _movementSpeed.Map(0, 100, 200, 10);

        while (true)
        {
            float seconds = (float)TimeSpan.FromMilliseconds(remapped).TotalSeconds;
            yield return new WaitForSecondsRealtime(seconds);

            // Translate without deltatime, not needed due to WaitForSecondsRealTime.
            _container.transform.Translate(Vector3.up);
        }
    }
}

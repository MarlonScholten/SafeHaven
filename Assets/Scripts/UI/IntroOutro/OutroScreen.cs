using SoundManager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroScreen : UIScreen
{
    [Header("Settings")]
    [SerializeField]
    [Range(1, 100)]
    [Tooltip("The speed at which the credits will scroll along, 1 is slow whilst 100 is fast.")]
    private float _movementSpeed;

    [SerializeField]
    [Range(1, 10)]
    [Tooltip("The time it will wait until returning to main menu after the credits are done scrolling.")]
    private float _timeAfterCredits;

    [Header("References")]
    [SerializeField]
    private RectTransform _container;

    private float _startY;

    private void Start()
    {
        Pause();

        _startY = _container.anchoredPosition.y;

        StartCoroutine(CreditCoroutine());
    }

    private IEnumerator CreditCoroutine()
    {
        // Remaps the clamped 0 to 100 range to 200 to 10.
        float remapped = _movementSpeed.Map(0, 100, 200, 10);

        // Loop whilst the scrollable hasn't reached the end.
        while (Mathf.Abs(_startY) >= _container.anchoredPosition.y)
        {
            float seconds = (float)TimeSpan.FromMilliseconds(remapped).TotalSeconds;
            yield return new WaitForSecondsRealtime(seconds);

            // Translate without deltatime, not needed due to WaitForSecondsRealTime.
            _container.transform.Translate(Vector3.up);
        }

        yield return new WaitForSeconds(_timeAfterCredits);

        // Return to main menu.
        Return();
    }
}

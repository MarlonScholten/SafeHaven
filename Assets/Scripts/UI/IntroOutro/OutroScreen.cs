using SoundManager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField]
    private Image _image;

    [SerializeField]
    private float _stopY = 2048;


    private void Start()
    {
        Pause();

        StartCoroutine(CreditCoroutine());
    }

    /// <summary>
    /// Sets the alpha of the backpanel.
    /// </summary>
    /// <param name="a">The alpha, between 0 and 255.</param>
    public void SetAlpha(float a)
    {
        Color temp = _image.color;
        temp.a = a;
        _image.color = temp;
    }

    private IEnumerator CreditCoroutine()
    {
        // Remaps the clamped 0 to 100 range to 200 to 10.
        float remapped = _movementSpeed.Map(0, 100, 200, 10);

        // Loop whilst the scrollable hasn't reached the end.
        while (_stopY >= _container.anchoredPosition.y)
        {
            float seconds = (float)TimeSpan.FromMilliseconds(remapped).TotalSeconds;
            yield return new WaitForSecondsRealtime(seconds);

            // Translate without deltatime, not needed due to WaitForSecondsRealTime.
            _container.transform.Translate(Vector3.up);
        }

        yield return new WaitForSecondsRealtime(_timeAfterCredits);

        // Return to main menu.
        Return();
    }
}

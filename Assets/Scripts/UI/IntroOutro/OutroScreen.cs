using System;
using System.Collections;
using UnityEngine;

public class OutroScreen : UIScreen
{
    [SerializeField]
    private float _movementSpeed = 10;

    [SerializeField]
    private RectTransform _container;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Pause();
        Debug.Log("Starting");
        StartCoroutine(CreditCoroutine());
    }

    private IEnumerator CreditCoroutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.01f);

            _container.transform.Translate(Vector3.up * Time.unscaledDeltaTime);

            //Vector2 pos = _container.anchoredPosition;
            //pos.y += _movementSpeed * Time.unscaledDeltaTime;
            //Debug.Log(_movementSpeed * Time.unscaledDeltaTime + " | " + Time.unscaledDeltaTime);
            //_container.anchoredPosition = pos;
        }
    }
}

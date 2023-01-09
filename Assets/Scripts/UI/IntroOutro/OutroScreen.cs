using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OutroScreen : UIScreen
{
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
            yield return new WaitForSecondsRealtime(0.2f);
            Vector2 pos = _container.position;
            _container.pos = new Vector2(pos.x, pos.y + 2 * Time.deltaTime);
            Debug.Log(pos);
        }
    }
}
